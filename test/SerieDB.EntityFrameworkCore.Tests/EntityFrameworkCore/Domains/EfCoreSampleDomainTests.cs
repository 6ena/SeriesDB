using SerieDB.Samples;
using Xunit;

namespace SerieDB.EntityFrameworkCore.Domains;

[Collection(SerieDBTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SerieDBEntityFrameworkCoreTestModule>
{

}
