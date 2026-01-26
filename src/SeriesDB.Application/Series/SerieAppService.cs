using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
        public SerieAppService(
            IRepository<Serie, int> repository,
            ISeriesApiService seriesApiService) 
        : base(repository)
        {
            _seriesApiService = seriesApiService;
        }

        public async Task<SerieDto[]> BuscarSerieAsync(string titulo, string genero = null)
        {
            return await _seriesApiService.BuscarSerieAsync(titulo, genero);
        }
    }
}
