using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity.Users;
using Volo.Abp.Validation;
using static Volo.Abp.Identity.Web.Pages.Identity.Users.CreateModalModel;

namespace Steer73.RockIT.Web.Pages.Identity.Users;

public class CustomCreateModalModel : IdentityUserModalPageModel
{
    #region Custom Code
    [BindProperty]
    public CustomUserInfoViewModel UserInfo { get; set; }
    #endregion CustomCode

    [BindProperty] public AssignedRoleViewModel[] Roles { get; set; }

    protected IIdentityUserAppService IdentityUserAppService { get; }

    public CustomCreateModalModel( 
        IIdentityUserAppService identityUserAppService)
    {
        IdentityUserAppService = identityUserAppService;
    }

    public async Task OnGetAsync()
    {
        #region Custom Code
        UserInfo = new CustomUserInfoViewModel();
        #endregion

        var roleDtoList = (await IdentityUserAppService.GetAssignableRolesAsync()).Items;

        Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(roleDtoList);

        OrganizationUnits =
            ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, AssignedOrganizationUnitViewModel[]>(
                (await IdentityUserAppService.GetAvailableOrganizationUnitsAsync()).Items
            );

        foreach (var role in Roles)
        {
            role.IsAssigned = role.IsDefault;
        }

        OrganizationUnitTreeRootNode = CreateOrganizationTree(new OrganizationTreeNode()
        {
            Id = null,
            Children = new List<OrganizationTreeNode>(),
            Index = -1
        });
    }

    public async Task<NoContentResult> OnPostAsync()
    {
        ValidateModel();

        #region Custom Code
        var input = ObjectMapper.Map<CustomUserInfoViewModel, IdentityUserCreateDto>(UserInfo);
        #endregion
        input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
        input.OrganizationUnitIds = OrganizationUnits.Where(ou => ou.IsAssigned).Select(ou => ou.Id).ToArray();

        await IdentityUserAppService.CreateAsync(input);

        return NoContent();
    }

    #region Custom Code
    public class CustomUserInfoViewModel : UserInfoViewModel
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
