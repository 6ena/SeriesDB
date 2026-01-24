using Volo.Abp.Modularity;

namespace SeriesDB;

[DependsOn(
    typeof(SeriesDBApplicationModule),
    typeof(SeriesDBDomainTestModule)
)]
public class SeriesDBApplicationTestModule : AbpModule
{

}
