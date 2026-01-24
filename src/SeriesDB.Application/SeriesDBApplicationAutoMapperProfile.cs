using AutoMapper;
using SeriesDB.Series;

namespace SeriesDB
{
    public class SeriesDBApplicationAutoMapperProfile : Profile
    {
        public SeriesDBApplicationAutoMapperProfile()
        {
            // Serie mappings
            CreateMap<Serie, SerieDto>();
            CreateMap<SerieDto, Serie>();
            CreateMap<CreateUpdateSerieDto, Serie>();
        }
    }
}
