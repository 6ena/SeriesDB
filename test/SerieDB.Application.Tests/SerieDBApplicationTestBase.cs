using Volo.Abp.Modularity;

namespace SerieDB;

public abstract class SerieDBApplicationTestBase<TStartupModule> : SerieDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
