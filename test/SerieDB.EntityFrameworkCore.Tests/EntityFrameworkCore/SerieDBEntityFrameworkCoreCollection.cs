using Xunit;

namespace SerieDB.EntityFrameworkCore;

[CollectionDefinition(SerieDBTestConsts.CollectionDefinitionName)]
public class SerieDBEntityFrameworkCoreCollection : ICollectionFixture<SerieDBEntityFrameworkCoreFixture>
{

}
