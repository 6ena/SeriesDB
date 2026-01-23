using SeriesDB.Samples;
using Xunit;

namespace SeriesDB.EntityFrameworkCore.Applications;

[Collection(SeriesDBTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SeriesDBEntityFrameworkCoreTestModule>
{

}
