using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
// using Volo.Abp.Account; // removed for OSS/no-auth
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Timing;
using Volo.Abp.Validation;

namespace Steer73.RockIT;

[AllowAnonymous]
[Dependency(ReplaceServices = true)]
[RemoteService(IsEnabled = false)]
public class CustomProfileAppService
{
    // Profile endpoints removed with no-auth/OSS

    // No profile update in no-auth mode

    private static void Validate(object input)
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
