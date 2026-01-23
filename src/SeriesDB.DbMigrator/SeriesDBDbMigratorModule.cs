using SeriesDB.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SeriesDB.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SeriesDBEntityFrameworkCoreModule),
    typeof(SeriesDBApplicationContractsModule)
)]
public class SeriesDBDbMigratorModule : AbpModule
{
}
