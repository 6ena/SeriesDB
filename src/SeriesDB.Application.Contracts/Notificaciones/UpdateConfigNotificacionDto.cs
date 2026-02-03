using System;

namespace SeriesDB.Application.Contracts.Notificaciones
{
    public class UpdateConfigNotificacionDto
    {
        public bool NotificacionPantalla { get; set; }
        public bool NotificacionEmail { get; set; }
    }
}