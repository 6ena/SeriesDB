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
                    Generos = "Generos Seed1"
                },
                autoSave: true
            );

            await _serieRepository.InsertAsync(
                new Serie
                {
                    Titulo = "Titulo Seed2",
                    Generos = "Generos Seed2"
                },
                autoSave: true
            );
        }
    }
}