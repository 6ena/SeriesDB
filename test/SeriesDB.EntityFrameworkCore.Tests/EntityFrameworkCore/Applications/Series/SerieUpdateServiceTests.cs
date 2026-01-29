using Moq;
using SeriesDB.Notificaciones;
using SeriesDB.Series;
using SeriesDB.ListasDeSeguimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;
using System.Reflection;

namespace SeriesDB.Tests.Series
{
    public class SerieUpdateServiceTests
    {
        private readonly Mock<ISeriesApiService> _seriesApiServiceMock;
        private readonly Mock<IRepository<Serie, int>> _serieRepositoryMock;
        private readonly Mock<IRepository<ListaDeSeguimiento, int>> _listaDeSeguimientoRepositoryMock;
        private readonly Mock<INotificacionAppService> _notificacionAppServiceMock;
        private readonly SerieUpdateService _serieUpdateService;

        public SerieUpdateServiceTests()
        {
            _seriesApiServiceMock = new Mock<ISeriesApiService>();
            _serieRepositoryMock = new Mock<IRepository<Serie, int>>();
            _listaDeSeguimientoRepositoryMock = new Mock<IRepository<ListaDeSeguimiento, int>>();
            _notificacionAppServiceMock = new Mock<INotificacionAppService>();

            _serieUpdateService = new SerieUpdateService(
                _seriesApiServiceMock.Object,
                _serieRepositoryMock.Object,
                _listaDeSeguimientoRepositoryMock.Object,
                _notificacionAppServiceMock.Object);
        }

        private static void SetEntityId<TEntity>(TEntity entity, int id) where TEntity : class
        {
            var idProperty = typeof(TEntity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(entity, id);
            }
            else
            {
                var idField = typeof(TEntity).GetProperty("Id");
                idField?.GetSetMethod(true)?.Invoke(entity, new object[] { id });
            }
        }

        [Fact]
        public async Task Should_Update_Series_When_New_Season_Available()
        {
            // Arrange
            var serieId = 1;
            var imdbId = "tt123456";
            var userId = Guid.NewGuid();

            var serie = new Serie
            {
                ImdbId = imdbId,
                Titulo = "Test Serie",
                TotalTemporadas = 1,
                Temporadas = new List<Temporada>()
            };
            SetEntityId(serie, serieId);

            var listaDeSeguimiento = new ListaDeSeguimiento
            {
                IdUsuario = userId,
                Series = new List<Serie> { serie }
            };
            SetEntityId(listaDeSeguimiento, 1);

            var listasDeSeguimiento = new List<ListaDeSeguimiento> { listaDeSeguimiento }.AsQueryable();

            _listaDeSeguimientoRepositoryMock
                .Setup(r => r.WithDetailsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ListaDeSeguimiento, object>>[]>()))
                .ReturnsAsync(listasDeSeguimiento);

            var apiSerie = new SerieDto { TotalTemporadas = 2, ImdbId = imdbId };

            _seriesApiServiceMock.Setup(s => s.BuscarSerieUpdateAsync(imdbId))
                .ReturnsAsync(apiSerie);

            var nuevaTemporada = new TemporadaDto
            {
                NroTemporada = 2,
                Episodios = new List<EpisodioDto>
                {
                    new EpisodioDto { NroEpisodio = 1, Titulo = "Nuevo episodio", FechaEstreno = DateOnly.FromDateTime(DateTime.Now) }
                }
            };

            _seriesApiServiceMock.Setup(s => s.BuscarTemporadaAsync(imdbId, 2))
                .ReturnsAsync(nuevaTemporada);

            // Act
            await _serieUpdateService.VerificarYActualizarSeriesAsync();

            // Assert
            _serieRepositoryMock.Verify(r => r.UpdateAsync(
                It.Is<Serie>(s => s.TotalTemporadas == 2 && s.Id == serieId), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()), 
                Times.Once);

            _notificacionAppServiceMock.Verify(n => n.CrearYEnviarNotificacionAsync(
                userId,
                $"Nueva temporada disponible de {serie.Titulo}",
                $"La temporada 2 ya está disponible en {serie.Titulo}.",
                TipoNotificacion.Email), Times.Once);

            _notificacionAppServiceMock.Verify(n => n.CrearYEnviarNotificacionAsync(
                userId,
                $"Nueva temporada disponible de {serie.Titulo}",
                $"La temporada 2 ya está disponible en {serie.Titulo}.",
                TipoNotificacion.Pantalla), Times.Once);
        }


        [Fact]
        public async Task Should_Notify_When_New_Episodes_Available()
        {
            // Arrange
            var serieId = 1;
            var temporadaId = 1;
            var imdbId = "tt123456";
            var userId = Guid.NewGuid();

            var temporada = new Temporada
            {
                NroTemporada = 1,
                Episodios = new List<Episodio>
                {
                    new Episodio { NroEpisodio = 1, Titulo = "Episodio 1", FechaEstreno = DateOnly.FromDateTime(DateTime.Now) }
                }
            };
            SetEntityId(temporada, temporadaId);

            var serie = new Serie
            {
                ImdbId = imdbId,
                Titulo = "Test Serie",
                TotalTemporadas = 1,
                Temporadas = new List<Temporada> { temporada }
            };
            SetEntityId(serie, serieId);

            var listaDeSeguimiento = new ListaDeSeguimiento
            {
                IdUsuario = userId,
                Series = new List<Serie> { serie }
            };
            SetEntityId(listaDeSeguimiento, 1);

            var listasDeSeguimiento = new List<ListaDeSeguimiento> { listaDeSeguimiento }.AsQueryable();

            _listaDeSeguimientoRepositoryMock
                .Setup(r => r.WithDetailsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ListaDeSeguimiento, object>>[]>()))
                .ReturnsAsync(listasDeSeguimiento);

            var apiSerie = new SerieDto { TotalTemporadas = 1, ImdbId = imdbId };

            _seriesApiServiceMock.Setup(s => s.BuscarSerieUpdateAsync(imdbId))
                .ReturnsAsync(apiSerie);

            var apiTemporada = new TemporadaDto
            {
                NroTemporada = 1,
                Episodios = new List<EpisodioDto>
                {
                    new EpisodioDto { NroEpisodio = 1, Titulo = "Episodio 1", FechaEstreno = DateOnly.FromDateTime(DateTime.Now) },
                    new EpisodioDto { NroEpisodio = 2, Titulo = "Nuevo Episodio 2", FechaEstreno = DateOnly.FromDateTime(DateTime.Now) }
                }
            };

            _seriesApiServiceMock.Setup(s => s.BuscarTemporadaAsync(imdbId, 1))
                .ReturnsAsync(apiTemporada);

            // Act
            await _serieUpdateService.VerificarYActualizarSeriesAsync();

            // Assert
            _serieRepositoryMock.Verify(r => r.UpdateAsync(
                It.Is<Serie>(s => s.Id == serieId), 
                It.IsAny<bool>(), 
                It.IsAny<CancellationToken>()), 
                Times.Once);

            _notificacionAppServiceMock.Verify(n => n.CrearYEnviarNotificacionAsync(
                userId,
                $"Nuevos episodios en {serie.Titulo}",
                "Se han añadido 1 nuevos episodios en la serie Test Serie.",
                TipoNotificacion.Email), Times.Once);

            _notificacionAppServiceMock.Verify(n => n.CrearYEnviarNotificacionAsync(
                userId,
                $"Nuevos episodios en {serie.Titulo}",
                "Se han añadido 1 nuevos episodios en la serie Test Serie.",
                TipoNotificacion.Pantalla), Times.Once);
        }
    }
}