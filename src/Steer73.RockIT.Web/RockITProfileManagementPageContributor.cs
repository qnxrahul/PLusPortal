using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Steer73.RockIT.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Public.Web.ProfileManagement;

namespace Steer73.RockIT.Web;

public class RockITProfileManagementPageContributor : IProfileManagementPageContributor
{
    public Task ConfigureAsync(ProfileManagementPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AccountResource>>();
        
        context.Groups.RemoveAll(g => g.Id == "Volo-Abp-Account-PersonalInfo");
        context.Groups.Add(
            new ProfileManagementPageGroup(
                "Volo-Abp-Account-PersonalInfo",
                l["ProfileTab:PersonalInfo"],
                typeof(CustomAccountProfilePersonalInfoManagementGroupViewComponent)));
        
        return Task.CompletedTask;
    }
}
