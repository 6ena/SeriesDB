using Volo.Abp.Modularity;

namespace SeriesDB;

/* Inherit from this class for your domain layer tests. */
public abstract class SeriesDBDomainTestBase<TStartupModule> : SeriesDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
