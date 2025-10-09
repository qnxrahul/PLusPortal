using Steer73.RockIT.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Steer73.RockIT.Web.Pages;

public abstract class RockITPageModel : AbpPageModel
{
    protected RockITPageModel()
    {
        LocalizationResourceType = typeof(RockITResource);
    }
}
