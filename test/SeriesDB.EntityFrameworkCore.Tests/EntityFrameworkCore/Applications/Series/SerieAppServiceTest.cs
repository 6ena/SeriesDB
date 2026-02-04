using Autofac.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using SeriesDB;
using SeriesDB.EntityFrameworkCore;
using SeriesDB.Series;
using SeriesDB.Usuarios;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
using Xunit;

namespace SeriesDB.Tests.Series
{
    public class SerieAppServiceTests
    {
        private readonly Mock<IRepository<Serie, int>> _serieRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<ISeriesApiService> _seriesApiServiceMock;
        private readonly Mock<IObjectMapper> _objectMapper;
        private readonly Mock<IMonitoreoApiAppService> _monitoreoApiAppService;
        private readonly SerieAppService _serieAppService;

        public SerieAppServiceTests()
        {
            _serieRepositoryMock = new Mock<IRepository<Serie, int>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _seriesApiServiceMock = new Mock<ISeriesApiService>();
            _objectMapper = new Mock<IObjectMapper>();
            _monitoreoApiAppService = new Mock<IMonitoreoApiAppService> { };
            _serieAppService = new SerieAppService(
                _serieRepositoryMock.Object,
                _seriesApiServiceMock.Object,
                _objectMapper.Object,
                _currentUserServiceMock.Object,
                _monitoreoApiAppService.Object
            );
        }

        [Fact]
        public async Task BuscarSerieAsync_ShouldReturnSeries_WhenSeriesExist()
        {
            // Arrange
            var titulo = "Test Title";
            var genero = "Test Genre";
            var series = new SerieDto[] { new SerieDto { Id = 1, Titulo = titulo } };

            _seriesApiServiceMock.Setup(s => s.BuscarSerieAsync(titulo, genero))
                .ReturnsAsync(series);

            // Act
            var result = await _serieAppService.BuscarSerieAsync(titulo, genero);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(titulo, result[0].Titulo);
        }

        /// <summary>
        /// Verifica que el método <c>BuscarTemporadaAsync</c> retorne una temporada 
        /// cuando existe una temporada que coincide con el ID de IMDb y el número de temporada proporcionados.
        /// </summary>
        [Fact]
        public async Task BuscarTemporadaAsync_ShouldReturnTemporada_WhenTemporadaExists()
        {
            // Arrange
            var imdbId = "tt1234567";
            var NroTemporada = 1;
            var temporada = new TemporadaDto { NroTemporada = NroTemporada };

            _seriesApiServiceMock.Setup(s => s.BuscarTemporadaAsync(imdbId, NroTemporada))
                .ReturnsAsync(temporada);

            // Act
            var result = await _serieAppService.BuscarTemporadaAsync(imdbId, NroTemporada);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(NroTemporada, result.NroTemporada);
        }

        //Tests para calificar serie

        /// <summary>
        /// Verifica que el método <c>CalificarSerieAsync</c> lance una excepción <see cref="InvalidOperationException"/> 
        /// cuando el usuario ya ha calificado la serie.
        /// </summary>
        [Fact]
        public async Task CalificarSerieAsync_ShouldThrowException_WhenUserAlreadyRated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>
                {
                    new Calificacion { IdUsuario = userId }
                }
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
            
            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Great series! Needs more Phillip, thou"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _serieAppService.CalificarSerieAsync(calificacionDto));
        }

        /// <summary>
        /// Verifica que el método <c>CalificarSerieAsync</c> agregue una nueva calificación 
        /// cuando el usuario no ha calificado la serie previamente.
        /// </summary>
        [Fact]
        public async Task CalificarSerieAsync_ShouldAddCalificacion_WhenUserHasNotRated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>()
            };
            
            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
            
            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Comentario de prueba"
            };

            // Act
            await _serieAppService.CalificarSerieAsync(calificacionDto);

            // Assert
            _serieRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Serie>(s => s.Calificaciones.Count == 1), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifica que el método <c>CalificarSerieAsync</c> agregue una nueva calificación 
        /// cuando el usuario no ha calificado la serie previamente (prueba de integración).
        /// </summary>
        [Fact]
        public async Task CalificarSerieAsync_ShouldAddCalificacion_WhenUserHasNotRated_Integration()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>()
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
            
            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Comentario de prueba"
            };

            // Act
            await _serieAppService.CalificarSerieAsync(calificacionDto);

            // Assert
            _serieRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Serie>(s => s.Calificaciones.Count == 1), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }



        //Tests para persistir serie

        /// <summary>
        /// Verifica que el método <c>PersistirSerieAsync</c> inserte una nueva serie cuando la serie no existe en el repositorio.
        /// </summary>
        [Fact]
        public async Task PersistirSerieAsync_ShouldInsertNewSerie_WhenSerieDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieDto = new SerieDto
            {
                Titulo = "Test 2",
                Clasificacion = "PG-13",
                FechaEstreno = "2023-01-09",
                Duracion = "2h",
                Generos = "Drama",
                Directores = "Director Test",
                Escritores = "Writer Test",
                Actores = "Actor Test",
                Sinopsis = "Test Sinopsis",
                Idiomas = "Español",
                Pais = "España",
                Poster = "URL del poster",
                ImdbCalificacion = "8.7",
                ImdbVotos = 1000,
                ImdbId = "tt1234567",
                Tipo = "Serie",
                TotalTemporadas = 3,
                Temporadas = new List<TemporadaDto>
        {
            new TemporadaDto { NroTemporada = 1, Titulo = "Temporada 1" },
            new TemporadaDto { NroTemporada = 2, Titulo = "Temporada 2" },
            new TemporadaDto { NroTemporada = 3, Titulo = "Temporada 3" },
        }
            };

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
            _serieRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<Expression<Func<Serie, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Serie>());
            _objectMapper.Setup(m => m.Map<SerieDto, Serie>(It.IsAny<SerieDto>())).Returns((SerieDto dto) => new Serie
            {
                Titulo = dto.Titulo,
                Clasificacion = dto.Clasificacion,
                FechaEstreno = dto.FechaEstreno,
                Duracion = dto.Duracion,
                Generos = dto.Generos,
                Directores = dto.Directores,
                Escritores = dto.Escritores,
                Actores = dto.Actores,
                Sinopsis = dto.Sinopsis,
                Idiomas = dto.Idiomas,
                Pais = dto.Pais,
                Poster = dto.Poster,
                ImdbCalificacion = dto.ImdbCalificacion,
                ImdbVotos = dto.ImdbVotos,
                ImdbId = dto.ImdbId,
                Tipo = dto.Tipo,
                TotalTemporadas = dto.TotalTemporadas
            });
            _objectMapper.Setup(m => m.Map<TemporadaDto, Temporada>(It.IsAny<TemporadaDto>())).Returns((TemporadaDto dto) => new Temporada
            {
                Titulo = dto.Titulo,
                FechaLanzamiento = dto.FechaLanzamiento,
                NroTemporada = dto.NroTemporada,
                SerieID = dto.SerieID
            });
            // Act
            await _serieAppService.PersistirSerieAsync(serieDto);

            // Assert
            _serieRepositoryMock.Verify(r => r.InsertAsync(It.Is<Serie>(s => s.ImdbId == "tt1234567" && s.TotalTemporadas == 3), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifica que se lance una InvalidOperationException cuando se intenta persistir una serie que ya está persistida.
        /// </summary>
        [Fact]
        public async Task PersistirSerieAsync_ShouldThrowInvalidOperationException_WhenSerieAlreadyPersisted()
        {
            // Arrange
            var serieDto = new SerieDto { ImdbId = "tt1234567", TotalTemporadas = 3 };

            var serieExistente = new Serie { ImdbId = "tt1234567", TotalTemporadas = 3 };
            _serieRepositoryMock.Setup(repo => repo.GetListAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Serie> { serieExistente });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _serieAppService.PersistirSerieAsync(serieDto));
            Assert.Equal("Serie ya esta persistida", exception.Message);
        }


        /// <summary>
        /// Verifica que el método <c>CalificarSerieAsync</c> lance una excepción <see cref="InvalidOperationException"/> 
        /// cuando el usuario ya ha calificado la serie (prueba de integración).
        /// </summary>
        [Fact]
        public async Task CalificarSerieAsync_ShouldThrowException_WhenUserAlreadyRated_Integration()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>
        {
            new Calificacion { IdUsuario = userId, NroCalificacion = 5, Comentario = "Great series!" }
        }
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);
            
            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Eggcellent series"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _serieAppService.CalificarSerieAsync(calificacionDto));
        }

        //Tests para modificar calificación

        /// <summary>
        /// Verifica que el método <c>ModificarCalificacionAsync</c> lance una excepción <see cref="EntityNotFoundException"/> 
        /// cuando la serie especificada no se encuentra en el repositorio.
        /// </summary>
        [Fact]
        public async Task ModificarCalificacionAsync_ShouldThrowException_WhenSerieNotFound()
        {
            // Arrange
            var calificacionDto = new CalificacionDto
            {
                SerieID = 1,
                NroCalificacion = 5,
                Comentario = "Great series!"
            };

            // Mock WithDetailsAsync to return an empty queryable
            var serieQueryable = new List<Serie>().AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _serieAppService.ModificarCalificacionAsync(calificacionDto));
        }

        /// <summary>
        /// Verifica que el método <c>ModificarCalificacionAsync</c> lance una excepción <see cref="InvalidOperationException"/> 
        /// cuando el usuario actual no puede ser encontrado.
        /// </summary>
        [Fact]
        public async Task ModificarCalificacionAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var serieId = 1;
            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 1,
                Comentario = "Awful series!"
            };

            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>()
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns((Guid?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _serieAppService.ModificarCalificacionAsync(calificacionDto));
        }

        /// <summary>
        /// Verifica que el método <c>ModificarCalificacionAsync</c> lance una excepción <see cref="InvalidOperationException"/> 
        /// cuando el usuario intenta modificar una calificación que no existe.
        /// </summary>
        [Fact]
        public async Task ModificarCalificacionAsync_ShouldThrowException_WhenUserNotAuthorized()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Great series!"
            };

            var serie = new Serie
            {
                Calificaciones = new List<Calificacion>()
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);

            // Act & Assert
            // The implementation throws InvalidOperationException when no calificación exists for the user
            await Assert.ThrowsAsync<InvalidOperationException>(() => _serieAppService.ModificarCalificacionAsync(calificacionDto));
        }

        /// <summary>
        /// Verifica que el método <c>ModificarCalificacionAsync</c> actualice correctamente la calificación y el comentario de una serie 
        /// cuando se proporcionan datos válidos. Además, verifica que el método <c>UpdateAsync</c> del repositorio sea llamado una vez.
        /// </summary>
        [Fact]
        public async Task ModificarCalificacionAsync_ShouldUpdateCalificacion_WhenValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serieId = 1;
            var calificacionDto = new CalificacionDto
            {
                SerieID = serieId,
                NroCalificacion = 5,
                Comentario = "Great series!"
            };

            var calificacionExistente = new Calificacion
            {
                IdUsuario = userId,
                NroCalificacion = 3,
                Comentario = "Good series"
            };

            var serie = new Serie
            {
                Calificaciones = new List<Calificacion> { calificacionExistente }
            };

            // Use reflection to set the protected Id property
            typeof(Serie).GetProperty("Id").SetValue(serie, serieId);

            // Mock WithDetailsAsync to return a queryable containing the serie
            var serieQueryable = new List<Serie> { serie }.AsQueryable();
            _serieRepositoryMock.Setup(r => r.WithDetailsAsync(It.IsAny<Expression<Func<Serie, object>>[]>()))
                .ReturnsAsync(serieQueryable);

            _currentUserServiceMock.Setup(s => s.GetCurrentUserId()).Returns(userId);

            // Act
            await _serieAppService.ModificarCalificacionAsync(calificacionDto);

            // Assert
            Assert.Equal(calificacionDto.NroCalificacion, calificacionExistente.NroCalificacion);
            Assert.Equal(calificacionDto.Comentario, calificacionExistente.Comentario);
            _serieRepositoryMock.Verify(r => r.UpdateAsync(serie, It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}