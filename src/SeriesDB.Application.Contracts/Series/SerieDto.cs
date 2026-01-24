using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SeriesDB.Series
{
    public class SerieDto : EntityDto<int>
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }
    }
}
