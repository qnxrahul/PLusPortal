using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Steer73.RockIT.Web;

[Dependency(ReplaceServices = true)]
public class RockITBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "RockIT";
}
