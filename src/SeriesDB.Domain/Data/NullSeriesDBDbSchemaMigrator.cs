using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SeriesDB.Data;

/* This is used if database provider does't define
 * ISeriesDBDbSchemaMigrator implementation.
 */
public class NullSeriesDBDbSchemaMigrator : ISeriesDBDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
