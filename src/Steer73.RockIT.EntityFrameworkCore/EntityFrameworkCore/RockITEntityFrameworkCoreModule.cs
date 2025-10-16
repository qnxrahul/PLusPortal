using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.JobFormResponses;
using Microsoft.Extensions.DependencyInjection;
using Steer73.RockIT.Companies;
using Steer73.RockIT.DiversityDatas;

using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
// using Volo.Abp.OpenIddict.EntityFrameworkCore; // removed for OSS/no-auth
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
// using Volo.Saas.EntityFrameworkCore; // removed for OSS
using Steer73.RockIT.BrochureSubscriptions;

namespace Steer73.RockIT.EntityFrameworkCore;

[DependsOn(
    typeof(RockITDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    // Remove Pro OpenIddict EFCore module for OSS migration
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    //typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    //typeof(LanguageManagementEntityFrameworkCoreModule),
    // Remove SaaS EFCore module for OSS migration
    //typeof(TextTemplateManagementEntityFrameworkCoreModule),
    //typeof(AbpGdprEntityFrameworkCoreModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule)
    )]
public class RockITEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RockITEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<RockITDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<Company, Companies.EfCoreCompanyRepository>();

            options.AddRepository<PracticeGroup, PracticeGroups.EfCorePracticeGroupRepository>();

            options.AddRepository<PracticeArea, PracticeAreas.EfCorePracticeAreaRepository>();

            options.AddRepository<Vacancy, Vacancies.EfCoreVacancyRepository>();

            options.AddRepository<FormDefinition, FormDefinitions.EfCoreFormDefinitionRepository>();

            options.AddRepository<JobApplication, JobApplications.EfCoreJobApplicationRepository>();

            options.AddRepository<DiversityData, DiversityDatas.EfCoreDiversityDataRepository>();

            options.AddRepository<JobFormResponse, JobFormResponses.EfCoreJobFormResponseRepository>();

            options.AddRepository<DiversityFormResponse, DiversityFormResponses.EfCoreDiversityFormResponseRepository>();

            options.AddRepository<BrochureSubscription, BrochureSubscriptions.EfCoreBrochureSubscriptionRepository>();

        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also RockITDbContextFactory for EF Core tooling. */
            options.UseSqlServer();
        });

    }
}