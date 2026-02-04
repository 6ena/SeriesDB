using System;

namespace SeriesDB.Application.Contracts.Notificaciones
{
    public class ConfigNotificacionDto
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public bool NotificacionPantalla { get; set; }
        public bool NotificacionEmail { get; set; }
    }
}