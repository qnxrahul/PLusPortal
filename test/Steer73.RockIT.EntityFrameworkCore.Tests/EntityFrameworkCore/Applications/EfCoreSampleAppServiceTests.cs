using Steer73.RockIT.Samples;
using Xunit;

namespace Steer73.RockIT.EntityFrameworkCore.Applications;

[Collection(RockITTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<RockITEntityFrameworkCoreTestModule>
{

}
