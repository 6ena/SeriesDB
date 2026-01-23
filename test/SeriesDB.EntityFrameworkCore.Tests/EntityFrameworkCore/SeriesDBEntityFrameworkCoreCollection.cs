using Xunit;

namespace SeriesDB.EntityFrameworkCore;

[CollectionDefinition(SeriesDBTestConsts.CollectionDefinitionName)]
public class SeriesDBEntityFrameworkCoreCollection : ICollectionFixture<SeriesDBEntityFrameworkCoreFixture>
{

}
