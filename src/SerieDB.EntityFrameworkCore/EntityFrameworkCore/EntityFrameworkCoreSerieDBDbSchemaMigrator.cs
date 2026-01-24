using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SerieDB.Data;
using Volo.Abp.DependencyInjection;

namespace SerieDB.EntityFrameworkCore;

public class EntityFrameworkCoreSerieDBDbSchemaMigrator
    : ISerieDBDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSerieDBDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SerieDBDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SerieDBDbContext>()
            .Database
            .MigrateAsync();
    }
}
