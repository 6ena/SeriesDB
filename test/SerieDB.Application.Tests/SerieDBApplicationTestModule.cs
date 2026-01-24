using Volo.Abp.Modularity;

namespace SerieDB;

[DependsOn(
    typeof(SerieDBApplicationModule),
    typeof(SerieDBDomainTestModule)
)]
public class SerieDBApplicationTestModule : AbpModule
{

}
