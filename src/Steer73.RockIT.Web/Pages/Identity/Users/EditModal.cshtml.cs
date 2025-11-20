using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Pages.Identity.Users;
using Volo.Abp.Validation;
using static Volo.Abp.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace Steer73.RockIT.Web.Pages.Identity.Users;

public class CustomEditModalModel : IdentityUserModalPageModel
{
    #region Custom Code
    [BindProperty] public CustomUserInfoViewModel UserInfo { get; set; }
    #endregion

    [BindProperty] public AssignedRoleViewModel[] Roles { get; set; }
    
    protected IIdentityUserAppService IdentityUserAppService { get; }

    protected IPermissionChecker PermissionChecker { get; }

    public CustomEditModalModel(
        IIdentityUserAppService identityUserAppService,
        IPermissionChecker permissionChecker)
    {
        IdentityUserAppService = identityUserAppService;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var user = await IdentityUserAppService.GetAsync(id);

        #region Custom Code
        UserInfo = ObjectMapper.Map<IdentityUserDto, CustomUserInfoViewModel>(user);
        #endregion

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
        {
            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>((await IdentityUserAppService.GetAssignableRolesAsync()).Items);

            var userRoleNames = (await IdentityUserAppService.GetRolesAsync(UserInfo.Id))
                .Items
                .Select(r => HttpUtility.HtmlEncode(r.Name))
                .ToList();

            foreach (var role in Roles)
            {
                if (userRoleNames.Contains(role.Name))
                {
                    role.IsAssigned = true;
                }
            }
        }

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageOU))
        {
            var userOrganizationUnits = (await IdentityUserAppService.GetOrganizationUnitsAsync(id)).ToList();

            OrganizationUnits =
                ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, AssignedOrganizationUnitViewModel[]>(
                    (await IdentityUserAppService.GetAvailableOrganizationUnitsAsync()).Items
                );

            var userOrganizationUnitIds = userOrganizationUnits.Select(ou => ou.Id).ToList();

            foreach (var ou in OrganizationUnits)
            {
                if (userOrganizationUnitIds.Contains(ou.Id))
                {
                    ou.IsAssigned = true;
                }
            }

            OrganizationUnitTreeRootNode = CreateOrganizationTree(new OrganizationTreeNode()
            {
                Id = null,
                Children = new List<OrganizationTreeNode>(),
                Index = -1
            });

            var userOrganizationUnitRoleIds = userOrganizationUnits.SelectMany(q => q.Roles)
                .Select(r => r.RoleId)
                .Distinct()
                .ToList();

            foreach (var role in Roles)
            {
                if (userOrganizationUnitRoleIds.Contains(role.Id))
                {
                    role.IsInheritedFromOu = true;
                }
            }
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        #region Custom Code
        var input = ObjectMapper.Map<CustomUserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
        #endregion
        input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
        input.OrganizationUnitIds = OrganizationUnits.Where(ou => ou.IsAssigned).Select(ou => ou.Id).ToArray();

        await IdentityUserAppService.UpdateAsync(UserInfo.Id, input);

        return NoContent();
    }

    private void HttpDecodeRoleNames()
    {
        if (Roles != null && Roles.Any())
        {
            foreach (var role in Roles)
            {
                role.Name = HttpUtility.HtmlDecode(role.Name);
            }
        }
    }
    
    private async Task<string> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }
        
        var user = await IdentityUserAppService.FindByIdAsync(userId.Value);
        return user?.UserName;
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
