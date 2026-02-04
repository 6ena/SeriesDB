using System;
using Volo.Abp.Domain.Entities;

namespace SeriesDB.Domain.Notificaciones
{
    public class ConfigNotificacion : Entity<Guid>
    {
        public Guid IdUsuario { get; set; }
        public bool NotificacionPantalla { get; set; }
        public bool NotificacionEmail { get; set; }

        protected ConfigNotificacion()
        {
            // Constructor protegido para EF Core
        }

        public ConfigNotificacion(Guid idUsuario, bool notificacionPantalla = true, bool notificacionEmail = false)
        {
            IdUsuario = idUsuario;
            NotificacionPantalla = notificacionPantalla;
            NotificacionEmail = notificacionEmail;
        }

        public void ActualizarPreferencias(bool notificacionPantalla, bool notificacionEmail)
        {
            NotificacionPantalla = notificacionPantalla;
            NotificacionEmail = notificacionEmail;
        }
    }
}