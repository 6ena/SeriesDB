using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Series
{
    public class Calificacion : Entity<int>
    {
        public float NroCalificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Foreign key
        public int SerieID { get; set; }
        public Serie Serie { get; set; }

        //Relación con el usuario que hizo la calificación
        public Guid IdUsuario { get; set; }
    }
}