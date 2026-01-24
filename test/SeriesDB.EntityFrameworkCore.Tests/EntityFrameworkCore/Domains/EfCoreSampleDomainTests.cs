using SeriesDB.Samples;
using Xunit;

namespace SeriesDB.EntityFrameworkCore.Domains;

[Collection(SeriesDBTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SeriesDBEntityFrameworkCoreTestModule>
{

}
