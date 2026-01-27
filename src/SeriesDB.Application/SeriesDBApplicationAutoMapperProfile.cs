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

            // Temporada mappings
            CreateMap<Temporada, TemporadaDto>();
            CreateMap<TemporadaDto, Temporada>();

            // Episodio mappings
            CreateMap<Episodio, EpisodioDto>();
            CreateMap<EpisodioDto, Episodio>();
        }
    }
}
