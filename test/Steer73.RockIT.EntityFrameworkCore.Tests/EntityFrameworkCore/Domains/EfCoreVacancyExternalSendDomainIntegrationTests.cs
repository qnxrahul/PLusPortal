using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steer73.RockIT.EntityFrameworkCore.Domains
{
    [Collection(RockITTestConsts.CollectionDefinitionName)]
    public class EfCoreVacancyExternalSendDomainIntegrationTests : VacancyExternalSendDomainIntegrationTests<RockITEntityFrameworkCoreTestModule>
    {
    }
}
