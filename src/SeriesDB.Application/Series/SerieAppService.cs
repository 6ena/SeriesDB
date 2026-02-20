using Microsoft.Extensions.Logging;
using SeriesDB.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace SeriesDB.Series
{
    public class SerieAppService : CrudAppService<
        Serie, // Entity
        SerieDto, // DTO to return
        int, // Primary key
        PagedAndSortedResultRequestDto, // Paging/sorting
        CreateUpdateSerieDto, // Create input
        CreateUpdateSerieDto>, // Update input
        ISerieAppService // Interface
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMonitoreoApiAppService _monitoreoApiAppService;
        private readonly ILogger<MonitoreoApiAppService> _logger;

        public SerieAppService(
            IRepository<Serie, int> repository,
            ISeriesApiService seriesApiService,
            IObjectMapper objectMapper,
            ICurrentUserService currentUserService,
            IMonitoreoApiAppService monitoreoApiAppService,
            ILogger<MonitoreoApiAppService> logger)
        : base(repository)
        {
            _seriesApiService = seriesApiService;
            _serieRepository = repository;
            _objectMapper = objectMapper;
            _currentUserService = currentUserService;
            _monitoreoApiAppService = monitoreoApiAppService;
            _logger = logger;
        }

        public async Task<SerieDto[]> BuscarSerieAsync(string titulo, string genero = null)
        {
            var monitoreo = await _monitoreoApiAppService.IniciarMonitoreo();
            try

            {
                var serie = await _seriesApiService.BuscarSerieAsync(titulo, genero);
                monitoreo = await _monitoreoApiAppService.FinalizarMonitoreo(monitoreo);
                await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo);
                return serie;
            }
            catch (Exception ex)
            {
                monitoreo = await _monitoreoApiAppService.ErrorMonitoreo(monitoreo, ex.Message);
                await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo);
                throw;
            }
        }

        public async Task<TemporadaDto> BuscarTemporadaAsync(string imdbId, int nroTemporada)
        {
            var monitoreo = await _monitoreoApiAppService.IniciarMonitoreo();

            try
            {
                var temporada = await _seriesApiService.BuscarTemporadaAsync(imdbId, nroTemporada);
                monitoreo = await _monitoreoApiAppService.FinalizarMonitoreo(monitoreo);
                await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo);
                return temporada;
            }
            catch (Exception ex)
            {
                monitoreo = await _monitoreoApiAppService.ErrorMonitoreo(monitoreo, ex.Message);
                await _monitoreoApiAppService.PersistirMonitoreoAsync(monitoreo);
                throw;
            }
        }


        private Serie MapSerieDtoToSerie(SerieDto serieDto)
        {
            var serie = _objectMapper.Map<SerieDto, Serie>(serieDto);
            serie.Temporadas = new List<Temporada>();

            if (serieDto.Temporadas != null)
            {
                foreach (var temporadaDto in serieDto.Temporadas)
                {
                    var temporada = _objectMapper.Map<TemporadaDto, Temporada>(temporadaDto);
                    serie.Temporadas.Add(temporada);
                }
            }

            return serie;
        }


        private void UpdateTemporadas(Serie serieExistente, List<TemporadaDto> temporadasDto)
        {
            if (temporadasDto != null)
            {
                foreach (var temporadaDto in temporadasDto)
                {
                    var temporadaExistente = serieExistente.Temporadas.FirstOrDefault(t => t.NroTemporada == temporadaDto.NroTemporada);
                    if (temporadaExistente == null)
                    {
                        var nuevaTemporada = _objectMapper.Map<TemporadaDto, Temporada>(temporadaDto);
                        serieExistente.Temporadas.Add(nuevaTemporada);
                    }
                    else
                    {
                        _objectMapper.Map(temporadaDto, temporadaExistente);
                    }
                }
            }
        }


        public async Task PersistirSerieAsync(SerieDto serieDto)
        {
            var seriesExistentes = await _serieRepository.GetListAsync();

            if (seriesExistentes == null)
            {
                seriesExistentes = new List<Serie>();
            }

            var serieExistente = seriesExistentes.FirstOrDefault(s => s.ImdbId == serieDto.ImdbId);

            if (serieExistente == null)
            {
                var nuevaSerie = MapSerieDtoToSerie(serieDto);
                await _serieRepository.InsertAsync(nuevaSerie);
            }
            else
            {
                if (serieExistente.TotalTemporadas == serieDto.TotalTemporadas)
                {
                    //demasiado extremo.
                    //localizar para el idioma del usuario.
                    throw new InvalidOperationException("Serie ya esta persistida");
                }
                else
                {
                    serieExistente.TotalTemporadas = serieDto.TotalTemporadas;
                    UpdateTemporadas(serieExistente, serieDto.Temporadas.ToList());
                    await _serieRepository.UpdateAsync(serieExistente);
                }
            }
        }


        public async Task CalificarSerieAsync(CalificacionDto input)
        {
            try
            {
                // Load the serie WITH its Calificaciones collection using ABP repository methods
                var queryable = await _serieRepository.WithDetailsAsync(s => s.Calificaciones);
                var serie = queryable.FirstOrDefault(s => s.Id == input.SerieID);

                if (serie == null)
                {
                    throw new EntityNotFoundException(typeof(Serie), input.SerieID);
                }

                var userIdActual = _currentUserService.GetCurrentUserId();
                if (!userIdActual.HasValue)
                {
                    throw new InvalidOperationException("User ID cannot be null");
                }

                var calificacionExistente = serie.Calificaciones.FirstOrDefault(c => c.IdUsuario == userIdActual.Value);
                if (calificacionExistente != null)
                {
                    throw new InvalidOperationException("Ya has calificado esta serie.");
                }

                var calificacion = new Calificacion
                {
                    NroCalificacion = input.NroCalificacion,
                    Comentario = input.Comentario,
                    FechaCreacion = DateTime.Now,
                    SerieID = input.SerieID,
                    IdUsuario = userIdActual.Value
                };

                serie.Calificaciones.Add(calificacion);
                await _serieRepository.UpdateAsync(serie);
                _logger.LogInformation("Serie calificada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calificar la serie.");
                throw;
            }
        }


        public async Task ModificarCalificacionAsync(CalificacionDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            try
            {
                var queryable = await _serieRepository.WithDetailsAsync(s => s.Calificaciones);
                var serie = queryable.FirstOrDefault(s => s.Id == input.SerieID);
                if (serie == null)
                {
                    throw new EntityNotFoundException(typeof(Serie), input.SerieID);
                }

                var userIdActual = _currentUserService.GetCurrentUserId();
                if (!userIdActual.HasValue)
                {
                    throw new InvalidOperationException("User ID cannot be null");
                }

                var calificacionExistente = serie.Calificaciones.FirstOrDefault(c => c.IdUsuario == userIdActual.Value);
                if (calificacionExistente == null)
                {
                    throw new InvalidOperationException("No hay calificación que modificar.");
                }

                calificacionExistente.NroCalificacion = input.NroCalificacion;
                calificacionExistente.Comentario = input.Comentario;
                calificacionExistente.FechaCreacion = DateTime.Now;

                await _serieRepository.UpdateAsync(serie);
                _logger.LogInformation("Calificación modificada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar la calificación.");
                throw;
            }
        }

    }
}