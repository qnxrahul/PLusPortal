using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Steer73.RockIT.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

[Dependency(ReplaceServices = true)]
public class CustomAccountProfilePersonalInfoManagementGroupViewComponent : Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo.AccountProfilePersonalInfoManagementGroupViewComponent
{
    public CustomAccountProfilePersonalInfoManagementGroupViewComponent(IProfileAppService profileAppService) : base(profileAppService)
    {}

    public override async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await ProfileAppService.GetAsync();

        #region Custom Code
        var model = ObjectMapper.Map<ProfileDto, CustomPersonalInfoModel>(user);
        #endregion

        if (model.SupportsMultipleTimezone)
        {
            model.TimeZoneItems = (await ProfileAppService.GetTimezonesAsync()).Select(x => new SelectListItem(x.Name, x.Value)).ToList();
        }

        return View("~/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.cshtml", model);
    }

    #region Custom Code
    public class CustomPersonalInfoModel : PersonalInfoModel
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public new string Name { get; set; } = "";

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public new string Surname { get; set; } = "";
    }
    #endregion
}
