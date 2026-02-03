using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SeriesDB.Domain.Notificaciones
{
    public interface IConfigNotificacionRepository : IRepository<ConfigNotificacion, Guid>
    {
        Task<ConfigNotificacion> GetByUsuarioIdAsync(Guid usuarioId);
    }
}