using SeriesDB.Application.Contracts.Notificaciones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesDB.Notificaciones
{
    public interface INotificacionAppService
    {
        List<NotificacionDto> MostrarNotificacionesPantalla(Guid idUsuario);
        Task CrearYEnviarNotificacionAsync(Guid idUsuario, string titulo, string mensaje, TipoNotificacion tipo);
        Task ModificarConfiguracionNotificacionAsync(Guid usuarioId, bool notificacionPantalla, bool notificacionEmail);
    }
}