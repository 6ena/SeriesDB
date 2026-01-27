using SeriesDB.Series;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace SeriesDB.ListasDeSeguimiento //corresponde a Watchlist, en el repo del profesor
{
    public class ListaDeSeguimiento : AggregateRoot<int>
    {
        public List<Serie> Series { get; set; } = new List<Serie>(); // Inicialización de la lista para evitar null reference
        public DateOnly FechaModificacion { get; set; }

        //Usuario
        public Guid UsuarioId { get; set; }
    }
}
