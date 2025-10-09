using Steer73.RockIT.Samples;
using Xunit;

namespace Steer73.RockIT.EntityFrameworkCore.Domains;

[Collection(RockITTestConsts.CollectionDefinitionName)]
public class EfCoreJobApplicationExternalSendDomainIntegrationTests : JobApplicationExternalSendDomainIntegrationTests<RockITEntityFrameworkCoreTestModule>
{

}
