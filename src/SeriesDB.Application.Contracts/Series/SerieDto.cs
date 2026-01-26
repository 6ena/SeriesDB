using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SeriesDB.Series
{
    public class SerieDto : EntityDto<int>
    {
        public string Titulo { get; set; }
        public string Generos { get; set; }
        public string Sinopsis { get; set; }
        public string FechaEstreno { get; set; }
        public string Duracion { get; set; }
        public string Clasificacion { get; set; }
        public string Idiomas { get; set; }
        public string Directores { get; set; }
        public string Escritores { get; set; }
        public string Actores { get; set; }
        public string Poster { get; set; }
        public string Pais { get; set; }
        public string ImdbId { get; set; }
        public string ImdbCalificacion { get; set; }
        public int ImdbVotos { get; set; }
        public string Tipo { get; set; }
        public int TotalTemporadas { get; set; }

        public ICollection<TemporadaDto> Temporadas { get; set; }
    }
}
