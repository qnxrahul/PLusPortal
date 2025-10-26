using Steer73.RockIT.Localization;
using Steer73.RockIT.Web.Pages.Identity.Users;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;
using static Steer73.RockIT.Web.Pages.Identity.Users.CustomCreateModalModel;

namespace Steer73.RockIT.Web;

public static class RockITViewModelExtensions
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
                /* You can add extension properties to DTOs
                 * defined in the depended modules.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */
        });

        ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
            [
                typeof(CustomCreateModalModel.CustomUserInfoViewModel),
                typeof(CustomEditModalModel.CustomUserInfoViewModel),
            ],
            CustomIdentityUserPropertyNames.CompanyRole,
            options =>
            {
                options.Attributes.Add(new StringLengthAttribute(255));
                options.DisplayName = LocalizableString.Create<RockITResource>(CustomIdentityUserPropertyNames.CompanyRole);
            });
    }
}
