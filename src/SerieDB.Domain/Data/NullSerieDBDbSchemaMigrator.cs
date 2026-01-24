using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SerieDB.Data;

/* This is used if database provider does't define
 * ISerieDBDbSchemaMigrator implementation.
 */
public class NullSerieDBDbSchemaMigrator : ISerieDBDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
