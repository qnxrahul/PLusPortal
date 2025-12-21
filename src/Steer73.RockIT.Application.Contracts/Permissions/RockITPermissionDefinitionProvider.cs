using Steer73.RockIT.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Steer73.RockIT.Permissions;

public class RockITPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(RockITSharedPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(RockITSharedPermissions.MyPermission1, L("Permission:MyPermission1"));

        //formsPermission.AddChild(RockITSharedPermissions.Forms.Edit, L("Permission:Edit"));
        //formsPermission.AddChild(RockITSharedPermissions.Forms.Delete, L("Permission:Delete"));

        //var vacancyPermission = myGroup.AddPermission(RockITSharedPermissions.Vacancy.VacancyGroup, L("Permission:Vacancy"));
        //vacancyPermission.AddChild(RockITSharedPermissions.Vacancy.Create, L("Permission:Create"));
        //vacancyPermission.AddChild(RockITSharedPermissions.Vacancy.View, L("Permission:View"));

        var companyPermission = myGroup.AddPermission(RockITSharedPermissions.Companies.Default, L("Permission:Companies"));
        companyPermission.AddChild(RockITSharedPermissions.Companies.Create, L("Permission:Create"));
        companyPermission.AddChild(RockITSharedPermissions.Companies.Edit, L("Permission:Edit"));
        companyPermission.AddChild(RockITSharedPermissions.Companies.Delete, L("Permission:Delete"));

        var practiceGroupPermission = myGroup.AddPermission(RockITSharedPermissions.PracticeGroups.Default, L("Permission:PracticeGroups"));
        practiceGroupPermission.AddChild(RockITSharedPermissions.PracticeGroups.Create, L("Permission:Create"));
        practiceGroupPermission.AddChild(RockITSharedPermissions.PracticeGroups.Edit, L("Permission:Edit"));
        practiceGroupPermission.AddChild(RockITSharedPermissions.PracticeGroups.Delete, L("Permission:Delete"));

        var practiceAreaPermission = myGroup.AddPermission(RockITSharedPermissions.PracticeAreas.Default, L("Permission:PracticeAreas"));
        practiceAreaPermission.AddChild(RockITSharedPermissions.PracticeAreas.Create, L("Permission:Create"));
        practiceAreaPermission.AddChild(RockITSharedPermissions.PracticeAreas.Edit, L("Permission:Edit"));
        practiceAreaPermission.AddChild(RockITSharedPermissions.PracticeAreas.Delete, L("Permission:Delete"));

        var vacancyPermission = myGroup.AddPermission(RockITSharedPermissions.Vacancies.Default, L("Permission:Vacancies"));
        vacancyPermission.AddChild(RockITSharedPermissions.Vacancies.Create, L("Permission:Create"));
        vacancyPermission.AddChild(RockITSharedPermissions.Vacancies.Edit, L("Permission:Edit"));
        vacancyPermission.AddChild(RockITSharedPermissions.Vacancies.Delete, L("Permission:Delete"));

        var formDefinitionPermission = myGroup.AddPermission(RockITSharedPermissions.FormDefinitions.Default, L("Permission:FormDefinitions"));
        formDefinitionPermission.AddChild(RockITSharedPermissions.FormDefinitions.Create, L("Permission:Create"));
        formDefinitionPermission.AddChild(RockITSharedPermissions.FormDefinitions.Edit, L("Permission:Edit"));
        formDefinitionPermission.AddChild(RockITSharedPermissions.FormDefinitions.Delete, L("Permission:Delete"));

        var vacancyFormDefinitionPermission = myGroup.AddPermission(RockITSharedPermissions.VacancyFormDefinitions.Default, L("Permission:VacancyFormDefinitions"));
        vacancyFormDefinitionPermission.AddChild(RockITSharedPermissions.VacancyFormDefinitions.Create, L("Permission:Create"));
        vacancyFormDefinitionPermission.AddChild(RockITSharedPermissions.VacancyFormDefinitions.Edit, L("Permission:Edit"));
        vacancyFormDefinitionPermission.AddChild(RockITSharedPermissions.VacancyFormDefinitions.Delete, L("Permission:Delete"));

        var jobApplicationPermission = myGroup.AddPermission(RockITSharedPermissions.JobApplications.Default, L("Permission:JobApplications"));
        jobApplicationPermission.AddChild(RockITSharedPermissions.JobApplications.Create, L("Permission:Create"));
        jobApplicationPermission.AddChild(RockITSharedPermissions.JobApplications.Edit, L("Permission:Edit"));
        jobApplicationPermission.AddChild(RockITSharedPermissions.JobApplications.Delete, L("Permission:Delete"));

        var diversityDataPermission = myGroup.AddPermission(RockITSharedPermissions.DiversityDatas.Default, L("Permission:DiversityDatas"));
        diversityDataPermission.AddChild(RockITSharedPermissions.DiversityDatas.Create, L("Permission:Create"));
        diversityDataPermission.AddChild(RockITSharedPermissions.DiversityDatas.Edit, L("Permission:Edit"));
        diversityDataPermission.AddChild(RockITSharedPermissions.DiversityDatas.Delete, L("Permission:Delete"));

        var jobFormResponsePermission = myGroup.AddPermission(RockITSharedPermissions.JobFormResponses.Default, L("Permission:JobFormResponses"));
        jobFormResponsePermission.AddChild(RockITSharedPermissions.JobFormResponses.Create, L("Permission:Create"));
        jobFormResponsePermission.AddChild(RockITSharedPermissions.JobFormResponses.Edit, L("Permission:Edit"));
        jobFormResponsePermission.AddChild(RockITSharedPermissions.JobFormResponses.Delete, L("Permission:Delete"));

        var diversityFormResponsePermission = myGroup.AddPermission(RockITSharedPermissions.DiversityFormResponses.Default, L("Permission:DiversityFormResponses"));
        diversityFormResponsePermission.AddChild(RockITSharedPermissions.DiversityFormResponses.Create, L("Permission:Create"));
        diversityFormResponsePermission.AddChild(RockITSharedPermissions.DiversityFormResponses.Edit, L("Permission:Edit"));
        diversityFormResponsePermission.AddChild(RockITSharedPermissions.DiversityFormResponses.Delete, L("Permission:Delete"));

        var jobAlertPermission = myGroup.AddPermission(RockITSharedPermissions.JobAlertRegistrations.Default, L("Permission:JobAlertRegistrations"));
        jobAlertPermission.AddChild(RockITSharedPermissions.JobAlertRegistrations.Manage, L("Permission:Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RockITResource>(name);
    }
}