using Steer73.RockIT.Constants;
using Steer73.RockIT.Permissions;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Data
{
    public class RolesDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        readonly IGuidGenerator _guidGenerator;
        readonly IdentityRoleManager _identityRoleManager;
        readonly IPermissionDataSeeder _permissionDataSeeder;
        readonly IPermissionGrantRepository _permissionGrantRepository;

        public RolesDataSeedContributor(
            IGuidGenerator guidGenerator,
            IdentityRoleManager identityRoleManager,
            IPermissionDataSeeder permissionDataSeeder,
            IPermissionGrantRepository permissionGrantRepository)
        {
            _guidGenerator = guidGenerator;
            _identityRoleManager = identityRoleManager;
            _permissionDataSeeder = permissionDataSeeder;
            _permissionGrantRepository = permissionGrantRepository;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var auditLogPermissions = new string[]
            {
                "AuditLogging.AuditLogs"
            };

            var companyPermissions = new string[]
            { 
                RockITSharedPermissions.Companies.Default
            };

            var diversityDataPermissions = new string[]
            {
                RockITSharedPermissions.DiversityDatas.Default
            };

            var formPermissions = new string[]
            {
                RockITSharedPermissions.FormDefinitions.Default,
                RockITSharedPermissions.VacancyFormDefinitions.Default,
            };

            var formManagementPermissions = new string[]
            {
                RockITSharedPermissions.FormDefinitions.Create,
                RockITSharedPermissions.FormDefinitions.Edit,
                RockITSharedPermissions.VacancyFormDefinitions.Create,
                RockITSharedPermissions.VacancyFormDefinitions.Edit
            };

            var jobApplicationPermissions = new string[]
            {  
                RockITSharedPermissions.JobApplications.Default,
                RockITSharedPermissions.JobApplications.Edit
            };

            var jobFormResponsePermissions = new string[]
            {
                RockITSharedPermissions.JobFormResponses.Default
            };

            var organizationUnitManagementPermissions = new string[]
            {
                "AbpIdentity.OrganizationUnits.ManageOU",
                "AbpIdentity.OrganizationUnits.ManageMembers"
            };

            var organizationUnitUserManagementPermissions = new string[]
            {
                "AbpIdentity.OrganizationUnits",
                "AbpIdentity.Users.Update.ManageOU"
            };

            var practiceAreaPermissions = new string[]
            {
                RockITSharedPermissions.PracticeAreas.Default
            };

            var practiceGroupPermissions = new string[]
            {
                RockITSharedPermissions.PracticeGroups.Default
            };

            var rolePermissions = new string[]
            {
                "AbpIdentity.Roles"
            };

            var securityLogPermissions = new string[]
            {
                "AbpIdentity.SecurityLogs"
            };

            var textTemplatePermissions = new string[]
            {
                "TextTemplateManagement.TextTemplates",
                "TextTemplateManagement.TextTemplates.EditContents"
            };

            var userManagementPermissions = new string[]
            {
                "AbpIdentity.Users",
                "AbpIdentity.Users.Create",
                "AbpIdentity.Users.Impersonation",
                "AbpIdentity.Users.Update",
                "AbpIdentity.Users.Update.ManageRoles",
                "AbpIdentity.Users.ViewDetails"
            };

            var vacancyPermissions = new string[]
            {
                RockITSharedPermissions.Vacancies.Default,
                RockITSharedPermissions.Vacancies.Create,
                RockITSharedPermissions.Vacancies.Edit
            };

            // Create or update roles
            await CreateOrUpdateRole(
                context,
                RoleConstants.Developer,
                [
                    ..auditLogPermissions,
                    ..companyPermissions,
                    ..formManagementPermissions,
                    ..formPermissions,
                    ..jobApplicationPermissions,
                    ..jobFormResponsePermissions,
                    ..organizationUnitManagementPermissions,
                    ..organizationUnitUserManagementPermissions,
                    ..practiceAreaPermissions,
                    ..practiceGroupPermissions,
                    ..rolePermissions,
                    ..securityLogPermissions,
                    ..textTemplatePermissions,
                    ..userManagementPermissions,
                    ..vacancyPermissions
                ]);

            await CreateOrUpdateRole(
                context,
                RoleConstants.Support,
                [
                    ..auditLogPermissions,
                    ..companyPermissions,
                    ..diversityDataPermissions,
                    ..formManagementPermissions,
                    ..formPermissions,
                    ..jobApplicationPermissions,
                    ..jobFormResponsePermissions,
                    ..practiceAreaPermissions,
                    ..practiceGroupPermissions,
                    ..rolePermissions,
                    ..securityLogPermissions,
                    ..textTemplatePermissions,
                    ..userManagementPermissions,
                    ..vacancyPermissions
                ]);

            await CreateOrUpdateRole(
                context,
                RoleConstants.HireManager,
                [
                    ..auditLogPermissions,
                    ..companyPermissions,
                    ..formManagementPermissions,
                    ..formPermissions,
                    ..jobApplicationPermissions,
                    ..jobFormResponsePermissions,
                    ..practiceAreaPermissions,
                    ..practiceGroupPermissions,
                    ..rolePermissions,
                    ..textTemplatePermissions,
                    ..userManagementPermissions,
                    ..vacancyPermissions
                ]);

            await CreateOrUpdateRole(
                context,
                RoleConstants.Recruiter,
                [
                    ..companyPermissions,
                    ..formPermissions,
                    ..jobApplicationPermissions,
                    ..jobFormResponsePermissions,
                    ..rolePermissions,
                    ..vacancyPermissions
                ],
                isDefault: true);
        }

        private async Task<IdentityRole> CreateOrUpdateRole(
            DataSeedContext context,
            string roleName,
            string[] permissionNames,
            bool isDefault = false)
        {
            var role = await _identityRoleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                var roleId = _guidGenerator.Create();
                role = new IdentityRole(
                    roleId,
                    roleName,
                    context.TenantId)
                {
                    IsStatic = true,
                    IsPublic = true,
                    IsDefault = isDefault
                };
                var identityResult = await _identityRoleManager.CreateAsync(role);
                if (!identityResult.Succeeded)
                {
                    throw new AbpIdentityResultException(identityResult);
                }

                await _permissionDataSeeder.SeedAsync(
                    RolePermissionValueProvider.ProviderName,
                    roleName,
                    permissionNames);
            }
            else
            {
                var existingPermissionGrants = await _permissionGrantRepository.GetListAsync(
                    RolePermissionValueProvider.ProviderName,
                    roleName);

                var existingPermissionGrantNames = existingPermissionGrants
                    .Select(x => x.Name)
                    .ToList();

                foreach (var permissionName in permissionNames.Except(existingPermissionGrantNames))
                {
                    await _permissionGrantRepository.InsertAsync(
                        new PermissionGrant(
                            _guidGenerator.Create(),
                            permissionName,
                            RolePermissionValueProvider.ProviderName,
                            roleName));
                }

                var permissionNamePairs = permissionNames.Distinct().ToDictionary(n => n);
                var permsissionGrantsToDelete = existingPermissionGrants
                      .Where(pg => !permissionNamePairs.ContainsKey(pg.Name))
                      .ToList();
                if (permsissionGrantsToDelete.Count > 0)
                {
                    await _permissionGrantRepository.DeleteManyAsync(permsissionGrantsToDelete);
                }
            }
         
            return role;
        }
    }
}
