using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Series
{
    public class Temporada : Entity<int>
    {
        public int NroTemporada { get; set; }
        public string Titulo { get; set; }
        public DateOnly FechaLanzamiento { get; set; }

        //Foreign key
        public int SerieID { get; set; }
        public Serie Serie { get; set; }

        //Relación uno a muchos con Episodio
        public ICollection<Episodio> Episodios { get; set; }
    }
}
