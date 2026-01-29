using System;
using System.Threading.Tasks;
using SeriesDB.Series;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SeriesDB.Usuarios;

public class SeriesDBDataSeederContributor
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Serie, int> _serieRepository;

    public SeriesDBDataSeederContributor(IRepository<Serie, int> serieRepository)
    {
        _serieRepository = serieRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _serieRepository.GetCountAsync() <= 0)
        {
            await _serieRepository.InsertAsync(
                new Serie
                {
                    Titulo = "Titulo Seed1",
                    Generos = "Generos Seed1",
                    Sinopsis = "Sinopsis de prueba para Seed1",
                    FechaEstreno = "2020-01-01",
                    Duracion = "45 min",
                    Clasificacion = "TV-14",
                    Idiomas = "Español",
                    Directores = "Director 1",
                    Escritores = "Escritor 1",
                    Actores = "Actor 1, Actor 2",
                    Poster = "https://example.com/poster1.jpg",
                    Pais = "Argentina",
                    ImdbId = "tt0000001",
                    ImdbCalificacion = "8.5",
                    ImdbVotos = 1000,
                    Tipo = "Series",
                    TotalTemporadas = 3
                },
                autoSave: true
            );

            await _serieRepository.InsertAsync(
                new Serie
                {
                    Titulo = "Titulo Seed2",
                    Generos = "Generos Seed2",
                    Sinopsis = "Sinopsis de prueba para Seed2",
                    FechaEstreno = "2021-06-15",
                    Duracion = "50 min",
                    Clasificacion = "TV-MA",
                    Idiomas = "Inglés",
                    Directores = "Director 2",
                    Escritores = "Escritor 2",
                    Actores = "Actor 3, Actor 4",
                    Poster = "https://example.com/poster2.jpg",
                    Pais = "Estados Unidos",
                    ImdbId = "tt0000002",
                    ImdbCalificacion = "7.8",
                    ImdbVotos = 500,
                    Tipo = "Series",
                    TotalTemporadas = 2
                },
                autoSave: true
            );
        }
    }
}