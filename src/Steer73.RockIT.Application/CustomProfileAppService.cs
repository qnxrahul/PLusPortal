using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Timing;
using Volo.Abp.Validation;

namespace Steer73.RockIT;

[AllowAnonymous]
[Dependency(ReplaceServices = true)]
[RemoteService(IsEnabled = false)]
public class CustomProfileAppService : ProfileAppService
{
    public CustomProfileAppService(
        IdentityUserManager userManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        IdentityProTwoFactorManager identityProTwoFactorManager,
        IOptions<IdentityOptions> identityOptions,
        IdentityUserTwoFactorChecker identityUserTwoFactorChecker,
        ITimezoneProvider timezoneProvider,
        ISettingManager settingManager) : base(
            userManager, 
            identitySecurityLogManager,
            identityProTwoFactorManager,
            identityOptions,
            identityUserTwoFactorChecker,
            timezoneProvider,
            settingManager) {}

    public override async Task<ProfileDto> UpdateAsync(UpdateProfileDto input)
    {
        Validate(input);

        return await base.UpdateAsync(input);
    }

    private static void Validate(UpdateProfileDto input)
    {
        var validationResults = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            validationResults.Add(new($"The {nameof(input.Name)} field is required", [nameof(input.Name)]));
        }

        if (string.IsNullOrWhiteSpace(input.Surname))
        {
            validationResults.Add(new($"The {nameof(input.Surname)} field is required", [nameof(input.Surname)]));
        }

        if (validationResults.Count > 0)
        {
            throw new AbpValidationException(validationResults);
        }
    }
}
