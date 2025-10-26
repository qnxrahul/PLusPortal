using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Security.Claims;
namespace Steer73.RockIT.Web.Pages.Account;

public class CustomRegisterModel : RegisterModel
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public CustomRegisterModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions, 
        IAccountExternalProviderAppService accountExternalProviderAppService, 
        ICurrentPrincipalAccessor currentPrincipalAccessor, 
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor) : base(
            schemeProvider, 
            accountOptions, 
            accountExternalProviderAppService, 
            currentPrincipalAccessor, 
            httpClientFactory) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<IActionResult> OnPostAsync()
    {
        return Redirect(await base.OnPostAsync());
    }

    private IActionResult Redirect(IActionResult result)
    {
        if (!IsExternalLogin
            || result is not RedirectResult redirectResult
            || string.IsNullOrWhiteSpace(redirectResult.Url)
            || redirectResult.Url != "/") { return result; }

        var canAccessAdminPortal = CurrentPrincipalAccessor.Principal.Claims
            .Any(c => c.Type == "role");
        if (canAccessAdminPortal)
        {
            redirectResult.Url = "~/admin/home";
        }

        return redirectResult;
    }
}
