using Volo.Abp.Modularity;

namespace SerieDB;

/* Inherit from this class for your domain layer tests. */
public abstract class SerieDBDomainTestBase<TStartupModule> : SerieDBTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
