using Microsoft.EntityFrameworkCore;
using SeriesDB.Domain.Notificaciones;
using SeriesDB.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SeriesDB.EntityFrameworkCore.Notificaciones
{
    public class NotificacionRepository : EfCoreRepository<SeriesDBDbContext, Notificacion, int>, INotificacionRepository // SeriesDBDbContext es el DbContext de la aplicación
                                                                                                                          //Notificacion es la entidad que maneja el repositorio
                                                                                                                          //int es el tipo de dato de la clave primaria de la entidad
    {
        public NotificacionRepository(IDbContextProvider<SeriesDBDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Notificacion>> GetNotificacionesNoLeidasAsync(Guid idUsuario)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .Where(n => n.IdUsuario == idUsuario && !n.Leida)
                .ToListAsync();
        }
    }
}