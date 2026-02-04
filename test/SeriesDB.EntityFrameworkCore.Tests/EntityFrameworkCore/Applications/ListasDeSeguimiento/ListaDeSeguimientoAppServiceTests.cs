using Microsoft.EntityFrameworkCore;
using SeriesDB.EntityFrameworkCore;
using SeriesDB.Series;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Xunit;

namespace SeriesDB.ListasDeSeguimiento
{
    public abstract class ListaDeSeguimientoAppServiceTests<TStartupModule> : SeriesDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        private readonly IListaDeSeguimientoAppService _listaDeSeguimientoAppService;
        private readonly SeriesDBDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        protected ListaDeSeguimientoAppServiceTests()
        {
            _listaDeSeguimientoAppService = GetRequiredService<IListaDeSeguimientoAppService>();
            _dbContext = GetRequiredService<SeriesDBDbContext>();
            _currentUser = GetRequiredService<ICurrentUser>();
        }

        /// <summary>
        /// Método helper para crear el usuario actual de prueba en la base de datos.
        /// </summary>
        protected async Task EnsureCurrentUserExistsAsync()
        {
            if (_currentUser.Id.HasValue)
            {
                var userExists = await _dbContext.Users.AnyAsync(u => u.Id == _currentUser.Id.Value);
                if (!userExists)
                {
                    var testUser = new IdentityUser(
                        _currentUser.Id.Value,
                        _currentUser.UserName ?? $"testuser_{_currentUser.Id.Value:N}",
                        _currentUser.Email ?? $"test_{_currentUser.Id.Value:N}@example.com"
                    );

                    await _dbContext.Users.AddAsync(testUser);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Verifica que el método <c>GetSeriesListaAsync</c> retorne una lista de series no vacía
        /// cuando se llama desde la lista de seguimiento del usuario actual.
        /// </summary>
        [Fact]
        public async Task GetSeriesListaAsync_Should_Show_Series_Of_The_List()
        {
            // Arrange: Asegurar que el usuario existe y agregar una serie a la lista primero
            await EnsureCurrentUserExistsAsync();

            var serieDto = new SerieDto
            {
                Titulo = "Test Series",
                Clasificacion = "PG-13",
                FechaEstreno = "2023-01-01",
                Duracion = "1h 30m",
                Generos = "Action",
                Directores = "Director Test",
                Escritores = "Writer Test",
                Actores = "Actor Test",
                Sinopsis = "Test Synopsis",
                Idiomas = "English",
                Pais = "USA",
                Poster = "URL del poster",
                ImdbCalificacion = "8.0",
                ImdbVotos = 1000,
                ImdbId = "tt1234567",
                Tipo = "Serie",
            };
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serieDto);

            // Act
            var seriesDto = await _listaDeSeguimientoAppService.GetSeriesListaAsync();

            // Assert
            seriesDto.ShouldNotBeEmpty();
            seriesDto.ShouldContain(s => s.ImdbId == "tt1234567");
        }

        /// <summary>
        /// Verifica que el método <c>AddSerieAsync</c> agregue correctamente una nueva serie a la lista de seguimiento
        /// y que la serie agregada esté presente en la base de datos con los datos correctos.
        /// </summary>
        [Fact]
        public async Task AddSerieListaAsync_Should_Add_One_Serie()
        {
            // Arrange: Asegurar que el usuario existe
            await EnsureCurrentUserExistsAsync();

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
                ImdbId = "xx54da154",
                Tipo = "Serie",
            };

            // Act
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serieDto);

            // Assert: verifica en la base de datos
            var serieEnDb = await _dbContext.Series
                .FirstOrDefaultAsync(s => s.ImdbId == "xx54da154");

            serieEnDb.ShouldNotBeNull(); // Verifica que la serie fue guardada
            serieEnDb.Titulo.ShouldBe("Test 2"); // Verifica que los datos coinciden
        }

        /// <summary>
        /// Verifica que el método <c>EliminarSerieAsync</c> elimine correctamente una serie de la lista de seguimiento
        /// y que la lista de series esté vacía después de la eliminación.
        /// </summary>
        [Fact]
        public async Task RemoveSerieListaAsync_Should_Erase_One_Serie()
        {
            // Arrange: Asegurar que el usuario existe y agregar una serie a la lista en un UoW separado
            await WithUnitOfWorkAsync(async () =>
            {
                await EnsureCurrentUserExistsAsync();

                var serieDto = new SerieDto
                {
                    Titulo = "Serie a eliminar",
                    Clasificacion = "PG-13",
                    FechaEstreno = "2023-01-01",
                    Duracion = "1h 30m",
                    Generos = "Action",
                    Directores = "Director Test",
                    Escritores = "Writer Test",
                    Actores = "Actor Test",
                    Sinopsis = "Test Synopsis",
                    Idiomas = "English",
                    Pais = "USA",
                    Poster = "URL del poster",
                    ImdbCalificacion = "8.0",
                    ImdbVotos = 1000,
                    ImdbId = "tt1234567",
                    Tipo = "Serie",
                };
                await _listaDeSeguimientoAppService.AddSerieListaAsync(serieDto);
            });

            // Act: Eliminar la serie en un UoW separado
            await WithUnitOfWorkAsync(async () =>
            {
                await _listaDeSeguimientoAppService.RemoveSerieListaAsync("tt1234567");
            });

            // Assert: Verificar que la lista está vacía en otro UoW
            await WithUnitOfWorkAsync(async () =>
            {
                var seriesDto = await _listaDeSeguimientoAppService.GetSeriesListaAsync();
                seriesDto.ShouldBeEmpty();
            });
        }

        /// <summary>
        /// Verifica que el método <c>BuscarSeriesListaAsync</c> busque series por título correctamente.
        /// </summary>
        [Fact]
        public async Task BuscarSeriesListaAsync_Should_Search_By_Title()
        {
            // Arrange: Asegurar que el usuario existe y agregar múltiples series
            await EnsureCurrentUserExistsAsync();

            var serie1 = new SerieDto
            {
                Titulo = "Breaking Bad",
                Clasificacion = "TV-MA",
                FechaEstreno = "2008-01-20",
                Duracion = "45m",
                Generos = "Crime, Drama, Thriller",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan",
                Actores = "Bryan Cranston, Aaron Paul",
                Sinopsis = "A chemistry teacher turned meth maker",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster1.jpg",
                ImdbCalificacion = "9.5",
                ImdbVotos = 1500000,
                ImdbId = "tt0903747",
                Tipo = "Serie",
            };

            var serie2 = new SerieDto
            {
                Titulo = "The Walking Dead",
                Clasificacion = "TV-MA",
                FechaEstreno = "2010-10-31",
                Duracion = "44m",
                Generos = "Drama, Horror, Thriller",
                Directores = "Frank Darabont",
                Escritores = "Frank Darabont",
                Actores = "Andrew Lincoln, Norman Reedus",
                Sinopsis = "Survivors in a zombie apocalypse",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster2.jpg",
                ImdbCalificacion = "8.2",
                ImdbVotos = 900000,
                ImdbId = "tt1520211",
                Tipo = "Serie",
            };

            var serie3 = new SerieDto
            {
                Titulo = "Better Call Saul",
                Clasificacion = "TV-MA",
                FechaEstreno = "2015-02-08",
                Duracion = "46m",
                Generos = "Crime, Drama",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan, Peter Gould",
                Actores = "Bob Odenkirk, Rhea Seehorn",
                Sinopsis = "The story of lawyer Saul Goodman",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster3.jpg",
                ImdbCalificacion = "9.0",
                ImdbVotos = 500000,
                ImdbId = "tt3032476",
                Tipo = "Serie",
            };

            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie1);
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie2);
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie3);

            // Act: Buscar por título "Breaking"
            var resultados = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("Breaking");

            // Assert
            resultados.ShouldNotBeEmpty();
            resultados.Length.ShouldBe(1);
            resultados[0].Titulo.ShouldBe("Breaking Bad");
        }

        /// <summary>
        /// Verifica que el método <c>BuscarSeriesListaAsync</c> busque series por género correctamente.
        /// </summary>
        [Fact]
        public async Task BuscarSeriesListaAsync_Should_Search_By_Genre()
        {
            // Arrange: Asegurar que el usuario existe y agregar múltiples series
            await EnsureCurrentUserExistsAsync();

            var serie1 = new SerieDto
            {
                Titulo = "Breaking Bad",
                Clasificacion = "TV-MA",
                FechaEstreno = "2008-01-20",
                Duracion = "45m",
                Generos = "Crime, Drama, Thriller",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan",
                Actores = "Bryan Cranston, Aaron Paul",
                Sinopsis = "A chemistry teacher turned meth maker",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster1.jpg",
                ImdbCalificacion = "9.5",
                ImdbVotos = 1500000,
                ImdbId = "tt0903747",
                Tipo = "Serie",
            };

            var serie2 = new SerieDto
            {
                Titulo = "Stranger Things",
                Clasificacion = "TV-14",
                FechaEstreno = "2016-07-15",
                Duracion = "51m",
                Generos = "Drama, Fantasy, Horror",
                Directores = "The Duffer Brothers",
                Escritores = "The Duffer Brothers",
                Actores = "Millie Bobby Brown, Finn Wolfhard",
                Sinopsis = "Kids in a small town uncover mysteries",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster4.jpg",
                ImdbCalificacion = "8.7",
                ImdbVotos = 1000000,
                ImdbId = "tt4574334",
                Tipo = "Serie",
            };

            var serie3 = new SerieDto
            {
                Titulo = "The Office",
                Clasificacion = "TV-14",
                FechaEstreno = "2005-03-24",
                Duracion = "22m",
                Generos = "Comedy",
                Directores = "Greg Daniels",
                Escritores = "Greg Daniels",
                Actores = "Steve Carell, Rainn Wilson",
                Sinopsis = "Mockumentary about office workers",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster5.jpg",
                ImdbCalificacion = "9.0",
                ImdbVotos = 600000,
                ImdbId = "tt0386676",
                Tipo = "Serie",
            };

            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie1);
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie2);
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie3);

            // Act: Buscar por género "Horror"
            var resultados = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("", "Horror");

            // Assert
            resultados.ShouldNotBeEmpty();
            resultados.Length.ShouldBe(1);
            resultados[0].Titulo.ShouldBe("Stranger Things");
        }

        /// <summary>
        /// Verifica que el método <c>BuscarSeriesListaAsync</c> busque series por título y género correctamente.
        /// </summary>
        [Fact]
        public async Task BuscarSeriesListaAsync_Should_Search_By_Title_And_Genre()
        {
            // Arrange: Asegurar que el usuario existe y agregar múltiples series
            await EnsureCurrentUserExistsAsync();

            var serie1 = new SerieDto
            {
                Titulo = "Breaking Bad",
                Clasificacion = "TV-MA",
                FechaEstreno = "2008-01-20",
                Duracion = "45m",
                Generos = "Crime, Drama, Thriller",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan",
                Actores = "Bryan Cranston, Aaron Paul",
                Sinopsis = "A chemistry teacher turned meth maker",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster1.jpg",
                ImdbCalificacion = "9.5",
                ImdbVotos = 1500000,
                ImdbId = "tt0903747",
                Tipo = "Serie",
            };

            var serie2 = new SerieDto
            {
                Titulo = "The Walking Dead",
                Clasificacion = "TV-MA",
                FechaEstreno = "2010-10-31",
                Duracion = "44m",
                Generos = "Drama, Horror, Thriller",
                Directores = "Frank Darabont",
                Escritores = "Frank Darabont",
                Actores = "Andrew Lincoln, Norman Reedus",
                Sinopsis = "Survivors in a zombie apocalypse",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster2.jpg",
                ImdbCalificacion = "8.2",
                ImdbVotos = 900000,
                ImdbId = "tt1520211",
                Tipo = "Serie",
            };

            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie1);
            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie2);

            // Act: Buscar por título "Walking" y género "Horror"
            var resultados = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("Walking", "Horror");

            // Assert
            resultados.ShouldNotBeEmpty();
            resultados.Length.ShouldBe(1);
            resultados[0].Titulo.ShouldBe("The Walking Dead");
        }

        /// <summary>
        /// Verifica que el método <c>BuscarSeriesListaAsync</c> retorne un array vacío cuando no hay coincidencias.
        /// </summary>
        [Fact]
        public async Task BuscarSeriesListaAsync_Should_Return_Empty_When_No_Matches()
        {
            // Arrange: Asegurar que el usuario existe y agregar series
            await EnsureCurrentUserExistsAsync();

            var serie = new SerieDto
            {
                Titulo = "Breaking Bad",
                Clasificacion = "TV-MA",
                FechaEstreno = "2008-01-20",
                Duracion = "45m",
                Generos = "Crime, Drama, Thriller",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan",
                Actores = "Bryan Cranston, Aaron Paul",
                Sinopsis = "A chemistry teacher turned meth maker",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster1.jpg",
                ImdbCalificacion = "9.5",
                ImdbVotos = 1500000,
                ImdbId = "tt0903747",
                Tipo = "Serie",
            };

            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie);

            // Act: Buscar con un título que no existe
            var resultados = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("NonExistentSeries");

            // Assert
            resultados.ShouldBeEmpty();
        }

        /// <summary>
        /// Verifica que el método <c>BuscarSeriesListaAsync</c> sea case-insensitive.
        /// </summary>
        [Fact]
        public async Task BuscarSeriesListaAsync_Should_Be_Case_Insensitive()
        {
            // Arrange: Asegurar que el usuario existe y agregar una serie
            await EnsureCurrentUserExistsAsync();

            var serie = new SerieDto
            {
                Titulo = "Breaking Bad",
                Clasificacion = "TV-MA",
                FechaEstreno = "2008-01-20",
                Duracion = "45m",
                Generos = "Crime, Drama, Thriller",
                Directores = "Vince Gilligan",
                Escritores = "Vince Gilligan",
                Actores = "Bryan Cranston, Aaron Paul",
                Sinopsis = "A chemistry teacher turned meth maker",
                Idiomas = "English",
                Pais = "USA",
                Poster = "poster1.jpg",
                ImdbCalificacion = "9.5",
                ImdbVotos = 1500000,
                ImdbId = "tt0903747",
                Tipo = "Serie",
            };

            await _listaDeSeguimientoAppService.AddSerieListaAsync(serie);

            // Act: Buscar con diferentes variaciones de mayúsculas/minúsculas
            var resultados1 = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("breaking");
            var resultados2 = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("BREAKING");
            var resultados3 = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("", "crime");
            var resultados4 = await _listaDeSeguimientoAppService.BuscarSeriesListaAsync("", "CRIME");

            // Assert
            resultados1.ShouldNotBeEmpty();
            resultados2.ShouldNotBeEmpty();
            resultados3.ShouldNotBeEmpty();
            resultados4.ShouldNotBeEmpty();

            resultados1[0].Titulo.ShouldBe("Breaking Bad");
            resultados2[0].Titulo.ShouldBe("Breaking Bad");
            resultados3[0].Titulo.ShouldBe("Breaking Bad");
            resultados4[0].Titulo.ShouldBe("Breaking Bad");
        }
    }


    // Concrete test class for Entity Framework Core
    public class ListaDeSeguimientoAppServiceTests_EntityFrameworkCore
        : ListaDeSeguimientoAppServiceTests<SeriesDBEntityFrameworkCoreTestModule>
    {
    }
}