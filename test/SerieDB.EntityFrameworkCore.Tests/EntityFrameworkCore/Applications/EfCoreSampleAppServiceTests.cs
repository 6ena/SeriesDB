using SerieDB.Samples;
using Xunit;

namespace SerieDB.EntityFrameworkCore.Applications;

[Collection(SerieDBTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SerieDBEntityFrameworkCoreTestModule>
{

}
