using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Identity;

namespace Steer73.RockIT.Web.Pages.Account;

public class CustomLogoutModel : LogoutModel
{

    public CustomLogoutModel() : base() { }

    public override async Task<IActionResult> OnGetAsync()
    {
        var result = await base.OnGetAsync();

        if (result is RedirectToPageResult redirectResult)
        {
            return Redirect(redirectResult.PageName ?? "~/Account/Login");
        }
        return result;
    }
}
