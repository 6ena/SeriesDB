using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Series
{
    public class Serie :AggregateRoot<int>
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }

    }
}
