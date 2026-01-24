using Volo.Abp.Modularity;

namespace SerieDB;

[DependsOn(
    typeof(SerieDBDomainModule),
    typeof(SerieDBTestBaseModule)
)]
public class SerieDBDomainTestModule : AbpModule
{

}
