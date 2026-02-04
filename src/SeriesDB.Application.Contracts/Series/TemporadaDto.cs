using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SeriesDB.Series
{
    public class TemporadaDto : EntityDto<int>
    {
        public int NroTemporada { get; set; }
        public string Titulo { get; set; }
        public DateOnly FechaLanzamiento { get; set; }

        //Foreign key
        public int SerieID { get; set; }

        //Relación uno a muchos con Episodio
        public ICollection<EpisodioDto> Episodios { get; set; }
    }
}
