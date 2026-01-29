using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SeriesDB.Domain.Notificaciones
{
    public interface INotificacionRepository : IRepository<Notificacion, int>
    {
        Task<List<Notificacion>> GetNotificacionesNoLeidasAsync(Guid idUsuario);
    }
}