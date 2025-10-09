using Localization.Resources.AbpUi;
using Steer73.RockIT.Localization;
// using Volo.Abp.Account; // removed for OSS/no-auth
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
// using Volo.Abp.LanguageManagement; // removed for OSS
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
// using Volo.Saas.Host; // removed for OSS
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
// using Volo.Abp.TextTemplateManagement; // removed for OSS
// using Volo.Abp.Gdpr; // removed for OSS
// using Volo.Abp.OpenIddict; // removed for OSS

namespace Steer73.RockIT;

 [DependsOn(
    typeof(RockITApplicationContractsModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAuditLoggingHttpApiModule)
    // Remove Pro OpenIddict & Account Admin for OSS/no-auth runtime
    )]
public class RockITHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<RockITResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
