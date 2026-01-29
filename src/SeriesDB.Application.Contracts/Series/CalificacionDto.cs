using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SeriesDB.Series
{
    public class CalificacionDto
    {
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public float NroCalificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Foreign key
        public int SerieID { get; set; }

        //Relación con el usuario que hizo la calificación
        public Guid IdUsuario { get; set; }
    }
}