using SeriesDB.Notificaciones;
using System;

namespace SeriesDB.Application.Contracts.Notificaciones
{
    public class NotificacionDto
    {
        public Guid IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leida { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}