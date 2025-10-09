using Xunit;

namespace Steer73.RockIT.EntityFrameworkCore;

[CollectionDefinition(RockITTestConsts.CollectionDefinitionName)]
public class RockITEntityFrameworkCoreCollection : ICollectionFixture<RockITEntityFrameworkCoreFixture>
{

}
