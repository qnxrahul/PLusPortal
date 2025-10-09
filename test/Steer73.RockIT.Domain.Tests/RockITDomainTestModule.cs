using Volo.Abp.Modularity;

namespace Steer73.RockIT;

[DependsOn(
    typeof(RockITDomainModule),
    typeof(RockITTestBaseModule)
)]
public class RockITDomainTestModule : AbpModule
{

}
