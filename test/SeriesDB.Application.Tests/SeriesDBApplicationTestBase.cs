using Volo.Abp.Modularity;

namespace SeriesDB;

public abstract class SeriesDBApplicationTestBase<TStartupModule> : SeriesDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
