using AutoMapper;
using SeriesDB.Application.Contracts.Notificaciones;
using SeriesDB.Domain.Notificaciones;
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

            //Notificacion mappings
            CreateMap<NotificacionDto, Notificacion>();

            //Monitoreo mappings
            CreateMap<MonitoreoApi, MonitoreoApiDto>();
            CreateMap<MonitoreoApiDto, MonitoreoApi>();
        }
    }
}
