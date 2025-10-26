using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Owl.reCAPTCHA;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Pages.Account;
using Volo.Abp.Account.Security.Recaptcha;
using Volo.Abp.Security.Claims;

namespace Steer73.RockIT.Web.Pages.Account
{
    public class CustomLoginModel : LoginModel
    {
        public CustomLoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions,
            IAbpRecaptchaValidatorFactory recaptchaValidatorFactory,
            IAccountExternalProviderAppService accountExternalProviderAppService,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IOptions<IdentityOptions> identityOptions,
            IOptionsSnapshot<reCAPTCHAOptions> reCaptchaOptions
            ) : base(schemeProvider, accountOptions, recaptchaValidatorFactory, accountExternalProviderAppService, currentPrincipalAccessor, identityOptions, reCaptchaOptions)
        {

        }

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            return Redirect(await base.OnPostAsync(action));
        }

        public override async Task<IActionResult> OnGetExternalLoginCallbackAsync(string remoteError = null)
        {
            return Redirect(await base.OnGetExternalLoginCallbackAsync(remoteError));
        }

        private IActionResult Redirect(IActionResult result)
        {
            if (result is not RedirectResult redirectResult
                || string.IsNullOrWhiteSpace(redirectResult.Url)
                || redirectResult.Url != "~/")
            {
                return result;
            }

            var canAccessAdminPortal = CurrentPrincipalAccessor.Principal.Claims
                .Any(c => c.Type == "role");
            if (canAccessAdminPortal)
            {
                redirectResult.Url = "~/admin/home";
            }
           
            return redirectResult;
        }
    }
}
