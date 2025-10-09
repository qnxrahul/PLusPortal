using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Steer73.RockIT;

[Authorize(IdentityPermissions.Users.Default)]
[Dependency(ReplaceServices = true)]
[RemoteService(IsEnabled = false)]
public class CustomIdentityUserAppService : IdentityUserAppService
{
    public CustomIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOrganizationUnitRepository organizationUnitRepository,
        IIdentityClaimTypeRepository identityClaimTypeRepository,
        IdentityProTwoFactorManager identityProTwoFactorManager,
        IOptions<IdentityOptions> identityOptions,
        IDistributedEventBus distributedEventBus,
        IOptions<AbpIdentityOptions> abpIdentityOptions,
        IPermissionChecker permissionChecker,
        IDistributedCache<IdentityUserDownloadTokenCacheItem, string> downloadTokenCache,
        IDistributedCache<ImportInvalidUsersCacheItem, string> importInvalidUsersCache,
        IdentitySessionManager identitySessionManager) : base(
            userManager,
            userRepository,
            roleRepository,
            organizationUnitRepository,
            identityClaimTypeRepository,
            identityProTwoFactorManager,
            identityOptions,
            distributedEventBus,
            abpIdentityOptions,
            permissionChecker,
            downloadTokenCache,
            importInvalidUsersCache,
            identitySessionManager)
    { }


    [Authorize(IdentityPermissions.Users.Create)]
    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        Validate(input);

        return await base.CreateAsync(input);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public override async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        Validate(input);

        return await base.UpdateAsync(id, input);
    }

    private static void Validate(IdentityUserCreateOrUpdateDtoBase input)
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
