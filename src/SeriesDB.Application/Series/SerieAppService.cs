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
        public SerieAppService(IRepository<Serie, int> repository) : base(repository)
        {
        }
    }
}
