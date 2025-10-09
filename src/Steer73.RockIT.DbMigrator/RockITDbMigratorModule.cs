using Steer73.RockIT.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Steer73.RockIT.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(RockITEntityFrameworkCoreModule),
    typeof(RockITApplicationContractsModule)
)]
public class RockITDbMigratorModule : AbpModule
{
}
