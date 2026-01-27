
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
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

        public SerieAppService(
            IRepository<Serie, int> repository,
            ISeriesApiService seriesApiService,
            IObjectMapper objectMapper)
        : base(repository)
        {
            _seriesApiService = seriesApiService;
            _serieRepository = repository;
            _objectMapper = objectMapper;
        }

        public async Task<SerieDto[]> BuscarSerieAsync(string titulo, string genero = null)
        {
            return await _seriesApiService.BuscarSerieAsync(titulo, genero);
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
    }
}