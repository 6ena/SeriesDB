using Microsoft.EntityFrameworkCore;
using SeriesDB.Domain.Notificaciones;
using SeriesDB.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SeriesDB.Repositories.Notificaciones
{
    public class ConfigNotificacionRepository : EfCoreRepository<SeriesDBDbContext, ConfigNotificacion, Guid>, IConfigNotificacionRepository
    {
        public ConfigNotificacionRepository(IDbContextProvider<SeriesDBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<ConfigNotificacion> GetByUsuarioIdAsync(Guid usuarioId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(c => c.IdUsuario == usuarioId);
        }
    }
}