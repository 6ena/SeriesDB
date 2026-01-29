using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using SeriesDB.Series;
using SeriesDB.Notificaciones;
using SeriesDB.ListasDeSeguimiento;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SeriesDB.Series
{
    public class SerieUpdateService : DomainService, ISerieUpdateService
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly IRepository<ListaDeSeguimiento, int> _listaDeSeguimientoRepository;
        private readonly INotificacionAppService _notificacionAppService;

        public SerieUpdateService(
            ISeriesApiService seriesApiService,
            IRepository<Serie, int> serieRepository,
            IRepository<ListaDeSeguimiento, int> listaDeSeguimientoRepository,
            INotificacionAppService notificacionAppService)
        {
            _seriesApiService = seriesApiService;
            _serieRepository = serieRepository;
            _listaDeSeguimientoRepository = listaDeSeguimientoRepository;
            _notificacionAppService = notificacionAppService;
        }

        public async Task VerificarYActualizarSeriesAsync()
        {
            // Obtener solo series que están en listas de seguimiento
            var queryable = await _listaDeSeguimientoRepository.WithDetailsAsync(x => x.Series);
            var todasLasListas = queryable.ToList();
            var seriesEnListas = queryable.SelectMany(l => l.Series)
                                           .DistinctBy(s => s.Id)
                                           .ToList();

            foreach (var serie in seriesEnListas)
            {
                try
                {
                    // Usar ImdbId para búsqueda precisa
                    var apiSerie = await _seriesApiService.BuscarSerieUpdateAsync(serie.ImdbId);
                    
                    if (apiSerie == null) continue;

                    // Validar Temporadas
                    if (serie.Temporadas == null)
                        serie.Temporadas = new List<Temporada>();

                    // Si la serie tiene más temporadas, se agrega la nueva temporada
                    if (apiSerie.TotalTemporadas > serie.TotalTemporadas)
                    {
                        var nuevaTemporadaNumero = serie.TotalTemporadas + 1;
                        var nuevaTemporadaApi = await _seriesApiService.BuscarTemporadaAsync(apiSerie.ImdbId, nuevaTemporadaNumero);

                        if (nuevaTemporadaApi != null)
                        {
                            var nuevaTemporada = new Temporada
                            {
                                NroTemporada = nuevaTemporadaNumero,
                                Episodios = nuevaTemporadaApi.Episodios.Select(e => new Episodio
                                {
                                    Titulo = e.Titulo,
                                    NroEpisodio = e.NroEpisodio,
                                    FechaEstreno = e.FechaEstreno
                                }).ToList()
                            };

                            serie.Temporadas.Add(nuevaTemporada);
                            serie.TotalTemporadas = apiSerie.TotalTemporadas;

                            await _serieRepository.UpdateAsync(serie);

                            // Notificar a usuarios que tienen la serie en su lista
                            await NotificarUsuariosConSerieAsync(
                                serie.Id,
                                $"Nueva temporada disponible de {serie.Titulo}",
                                $"La temporada {nuevaTemporadaNumero} ya está disponible en {serie.Titulo}.",
                                todasLasListas);
                        }
                    }

                    // Obtener la última temporada local
                    var ultimaTemporadaLocal = serie.Temporadas.OrderByDescending(t => t.NroTemporada).FirstOrDefault();
                    if (ultimaTemporadaLocal != null)
                    {
                        // Obtener la última temporada desde la API
                        var apiUltimaTemporada = await _seriesApiService.BuscarTemporadaAsync(apiSerie.ImdbId, ultimaTemporadaLocal.NroTemporada);

                        if (apiUltimaTemporada != null)
                        {
                            // Comparar la cantidad de episodios
                            if (apiUltimaTemporada.Episodios.Count > ultimaTemporadaLocal.Episodios.Count)
                            {
                                // Detectar episodios nuevos
                                var episodiosLocales = ultimaTemporadaLocal.Episodios.Select(e => e.NroEpisodio).ToHashSet();
                                var episodiosNuevos = apiUltimaTemporada.Episodios
                                    .Where(e => !episodiosLocales.Contains(e.NroEpisodio))
                                    .ToList();

                                if (episodiosNuevos.Any())
                                {
                                    // Lógica para manejar los episodios nuevos
                                    foreach (var episodioNuevo in episodiosNuevos)
                                    {
                                        var nuevoEpisodio = new Episodio
                                        {
                                            Titulo = episodioNuevo.Titulo,
                                            NroEpisodio = episodioNuevo.NroEpisodio,
                                            FechaEstreno = episodioNuevo.FechaEstreno,
                                            TemporadaID = ultimaTemporadaLocal.Id
                                        };

                                        // Agregar a la colección de episodios de la temporada local
                                        ultimaTemporadaLocal.Episodios.Add(nuevoEpisodio);
                                    }

                                    await _serieRepository.UpdateAsync(serie);

                                    // Notificar a usuarios que tienen la serie en su lista
                                    await NotificarUsuariosConSerieAsync(
                                        serie.Id,
                                        $"Nuevos episodios en {serie.Titulo}",
                                        $"Se han añadido {episodiosNuevos.Count} nuevos episodios en la serie {serie.Titulo}.",
                                        todasLasListas);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error al actualizar serie {SerieId} - {Titulo}", serie.Id, serie.Titulo);
                }
            }
        }

        private async Task NotificarUsuariosConSerieAsync(int serieId, string titulo, string mensaje, List<ListaDeSeguimiento> todasLasListas)
        {
            var listasConSerie = todasLasListas.Where(l => l.Series.Any(s => s.Id == serieId)).ToList();

            // Notificar a cada usuario que tiene la serie
            foreach (var lista in listasConSerie)
            {
                await _notificacionAppService.CrearYEnviarNotificacionAsync(
                    lista.IdUsuario, titulo, mensaje, TipoNotificacion.Email);
                await _notificacionAppService.CrearYEnviarNotificacionAsync(
                    lista.IdUsuario, titulo, mensaje, TipoNotificacion.Pantalla);
            }
        }
    }
}