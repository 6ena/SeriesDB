using SerieDB.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SerieDB.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SerieDBEntityFrameworkCoreModule),
    typeof(SerieDBApplicationContractsModule)
)]
public class SerieDBDbMigratorModule : AbpModule
{
}
