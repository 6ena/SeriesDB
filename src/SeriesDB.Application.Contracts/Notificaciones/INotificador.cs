using SeriesDB.Notificaciones;
using System.Threading.Tasks;

namespace SeriesDB.Application.Contracts.Notificaciones
{
    public interface INotificador
    {
        bool PuedeEnviar(TipoNotificacion tipo);
        Task EnviarNotificacionAsync(NotificacionDto notificacion);
    }
}