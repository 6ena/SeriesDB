using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Series
{
    public class Episodio : Entity<int>
    {
        public int NroEpisodio { get; set; }
        public string Titulo { get; set; }
        public string Duracion { get; set; }
        public string Resumen { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public string Directores { get; set; }
        public string Escritores { get; set; }

        //Foreign key
        public int TemporadaID { get; set; }
        public Temporada Temporada { get; set; }
    }
}
