// using Volo.Abp.Account; // removed for OSS/no-auth
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
// using Volo.Saas.Host; // removed for OSS
// using Volo.Abp.OpenIddict; // removed for OSS

namespace Steer73.RockIT;

[DependsOn(
    typeof(RockITDomainSharedModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule)
)]
public class RockITApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RockITDtoExtensions.Configure();
    }
}
