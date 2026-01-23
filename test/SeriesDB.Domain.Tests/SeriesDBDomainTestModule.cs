using Volo.Abp.Modularity;

namespace SeriesDB;

[DependsOn(
    typeof(SeriesDBDomainModule),
    typeof(SeriesDBTestBaseModule)
)]
public class SeriesDBDomainTestModule : AbpModule
{

}
