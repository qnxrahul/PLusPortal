using Steer73.RockIT.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Steer73.RockIT.EntityFrameworkCore.Domains;

[Collection(RockITTestConsts.CollectionDefinitionName)]
public class EFCoreCompanyListUpdateTests : CompanyListUpdateDomainIntegrationTests<RockITEntityFrameworkCoreTestModule>
{

}
