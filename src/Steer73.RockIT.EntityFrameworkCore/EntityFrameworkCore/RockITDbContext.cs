using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.JobFormResponses;
using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.Companies;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.BrochureSubscriptions;
using Steer73.RockIT.MediaSources;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Gdpr;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Tenants;
using Steer73.RockIT.Enums;
using System;
using Steer73.RockIT.RoleTypes;

namespace Steer73.RockIT.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class RockITDbContext :
    AbpDbContext<RockITDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    public DbSet<DiversityFormResponse> DiversityFormResponses { get; set; } = null!;
    public DbSet<JobFormResponse> JobFormResponses { get; set; } = null!;
    public DbSet<DiversityData> DiversityDatas { get; set; } = null!;
    public DbSet<JobApplication> JobApplications { get; set; } = null!;
    public DbSet<FormDefinition> FormDefinitions { get; set; } = null!;
    public DbSet<AppFileDescriptors.AppFileDescriptor> AppFileDescriptors { get; set; } = null!;
    public DbSet<Vacancy> Vacancies { get; set; } = null!;
    public DbSet<PracticeArea> PracticeAreas { get; set; } = null!;
    public DbSet<PracticeGroup> PracticeGroups { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<BrochureSubscription> BrochureSubscriptions { get; set; } = null!;
    public DbSet<MediaSource> MediaSources { get; set; } = null!;
    public DbSet<VacancyMediaSource> VacancyMediaSources { get; set; } = null!;
    public DbSet<RoleType> RoleTypes { get; set; } = null!;
    public DbSet<VacancyRoleType> VacancyRoleTypes { get; set; } = null!;

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public RockITDbContext(DbContextOptions<RockITDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureGdpr();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(RockITConsts.DbTablePrefix + "YourEntities", RockITConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        if (builder.IsHostDatabase())
        {
            builder.Entity<Company>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "Companies", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(Company.Name)).HasMaxLength(CompanyConsts.NameMaxLength);
                b.Property(x => x.Phone).HasColumnName(nameof(Company.Phone)).HasMaxLength(CompanyConsts.PhoneMaxLength);
                b.Property(x => x.Address).HasColumnName(nameof(Company.Address)).HasMaxLength(CompanyConsts.AddressMaxLength);
                b.Property(x => x.Postcode).HasColumnName(nameof(Company.Postcode)).HasMaxLength(CompanyConsts.PostcodeMaxLength);
                b.Property(x => x.PrimaryContact).HasColumnName(nameof(Company.PrimaryContact)).HasMaxLength(CompanyConsts.PrimaryContactMaxLength);
                b.Property(x => x.LogoUrl).HasColumnName(nameof(Company.LogoUrl)).HasMaxLength(CompanyConsts.LogoUrlMaxLength);
            });

            builder.Entity<PracticeGroup>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "PracticeGroups", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(PracticeGroup.Name)).IsRequired().HasMaxLength(PracticeGroupConsts.NameMaxLength);
                b.Property(x => x.IsActive).HasColumnName(nameof(PracticeGroup.IsActive));
                b.HasMany(x => x.PracticeAreas).WithOne().HasForeignKey(x => x.PracticeGroupId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasMany<VacancyPracticeGroup>().WithOne(x => x.PracticeGroup).HasForeignKey(x => x.PracticeGroupId).IsRequired();
            });

            builder.Entity<PracticeArea>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "PracticeAreas", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(PracticeArea.Name)).IsRequired().HasMaxLength(PracticeAreaConsts.NameMaxLength);
                b.Property(x => x.IsActive).HasColumnName(nameof(PracticeArea.IsActive));
                b.HasOne<PracticeGroup>().WithMany(x => x.PracticeAreas).HasForeignKey(x => x.PracticeGroupId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<FormDefinition>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "FormDefinitions", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ReferenceId).HasColumnName(nameof(FormDefinition.ReferenceId)).IsRequired().HasMaxLength(FormDefinitionConsts.ReferenceIdMaxLength);
                b.Property(x => x.Name).HasColumnName(nameof(FormDefinition.Name)).IsRequired().HasMaxLength(FormDefinitionConsts.NameMaxLength);
                b.Property(x => x.FormDetails).HasColumnName(nameof(FormDefinition.FormDetails));
                b.Property(x => x.FormType).HasColumnName(nameof(FormDefinition.FormType));
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<DiversityData>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "DiversityDatas", RockITConsts.DiversityDbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.HappyToCompleteForm).HasColumnName(nameof(DiversityData.HappyToCompleteForm));
                b.Property(x => x.AgeRange).HasColumnName(nameof(DiversityData.AgeRange));
                b.Property(x => x.Gender).HasColumnName(nameof(DiversityData.Gender));
                b.Property(x => x.OtherGender).HasColumnName(nameof(DiversityData.OtherGender)).HasMaxLength(DiversityDataConsts.OtherGenderMaxLength);
                b.Property(x => x.GenderIdentitySameAsBirth).HasColumnName(nameof(DiversityData.GenderIdentitySameAsBirth));
                b.Property(x => x.Sex).HasColumnName(nameof(DiversityData.Sex));
                b.Property(x => x.OtherSex).HasColumnName(nameof(DiversityData.OtherSex)).HasMaxLength(DiversityDataConsts.OtherSexMaxLength);
                b.Property(x => x.SexualOrientation).HasColumnName(nameof(DiversityData.SexualOrientation));
                b.Property(x => x.OtherSexualOrientation).HasColumnName(nameof(DiversityData.OtherSexualOrientation)).HasMaxLength(DiversityDataConsts.OtherSexualOrientationMaxLength);
                b.Property(x => x.Ethnicity).HasColumnName(nameof(DiversityData.Ethnicity));
                b.Property(x => x.OtherEthnicity).HasColumnName(nameof(DiversityData.OtherEthnicity)).HasMaxLength(DiversityDataConsts.OtherEthnicityMaxLength);
                b.Property(x => x.ReligionOrBelief).HasColumnName(nameof(DiversityData.ReligionOrBelief));
                b.Property(x => x.OtherReligionOrBelief).HasColumnName(nameof(DiversityData.OtherReligionOrBelief)).HasMaxLength(DiversityDataConsts.OtherReligionOrBeliefMaxLength);
                b.Property(x => x.Disability).HasColumnName(nameof(DiversityData.Disability));
                b.Property(x => x.EducationLevel).HasColumnName(nameof(DiversityData.EducationLevel));
                b.Property(x => x.TypeOfSecondarySchool).HasColumnName(nameof(DiversityData.TypeOfSecondarySchool));
                b.Property(x => x.OtherTypeOfSecondarySchool).HasColumnName(nameof(DiversityData.OtherTypeOfSecondarySchool)).HasMaxLength(DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength);
                b.Property(x => x.HigherEducationQualifications).HasColumnName(nameof(DiversityData.HigherEducationQualifications));
                b.HasOne<JobApplication>().WithMany(x => x.DiversityDatas).HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<JobFormResponse>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "JobFormResponses", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FormStructureJson).HasColumnName(nameof(JobFormResponse.FormStructureJson)).IsRequired();
                b.Property(x => x.FormResponseJson).HasColumnName(nameof(JobFormResponse.FormResponseJson)).IsRequired();
                b.HasOne<JobApplication>().WithMany(x => x.JobFormResponses).HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<JobApplication>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "JobApplications", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(JobApplication.FirstName)).IsRequired().HasMaxLength(JobApplicationConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(JobApplication.LastName)).IsRequired().HasMaxLength(JobApplicationConsts.LastNameMaxLength);
                b.Property(x => x.Aka).HasColumnName(nameof(JobApplication.Aka)).IsRequired().HasMaxLength(JobApplicationConsts.DefaultStringMaxLength);
                b.Property(x => x.EmailAddress).HasColumnName(nameof(JobApplication.EmailAddress)).IsRequired().HasMaxLength(JobApplicationConsts.EmailAddressMaxLength);
                b.Property(x => x.Title).HasColumnName(nameof(JobApplication.Title)).HasMaxLength(JobApplicationConsts.TitleMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(JobApplication.Status)).HasDefaultValue(JobApplicationStatus.Pending).HasSentinel(JobApplicationStatus.Pending);
                b.Property(x => x.SyncStatus).HasColumnName(nameof(JobApplication.SyncStatus)).HasDefaultValue(SyncStatus.Pending).HasSentinel(SyncStatus.Pending);
                b.Property(x => x.ApproveEmailStatus).HasColumnName(nameof(JobApplication.ApproveEmailStatus)).HasDefaultValue(SyncStatus.Pending).HasSentinel(SyncStatus.Pending);
                b.Property(x => x.RejectEmailStatus).HasColumnName(nameof(JobApplication.RejectEmailStatus)).HasDefaultValue(SyncStatus.Pending).HasSentinel(SyncStatus.Pending);
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(JobApplication.PhoneNumber)).HasMaxLength(JobApplicationConsts.PhoneNumberMaxLength);
                b.Property(x => x.Landline).HasColumnName(nameof(JobApplication.Landline)).HasMaxLength(JobApplicationConsts.LandlineMaxLength);
                b.Property(x => x.CurrentRole).HasColumnName(nameof(JobApplication.CurrentRole)).HasMaxLength(JobApplicationConsts.CurrentRoleMaxLength);
                b.Property(x => x.CurrentCompany).HasColumnName(nameof(JobApplication.CurrentCompany)).HasMaxLength(JobApplicationConsts.CurrentCompanyMaxLength);
                b.Property(x => x.CurrentPositionType).HasColumnName(nameof(JobApplication.CurrentPositionType)).HasMaxLength(JobApplicationConsts.CurrentPositionTypeMaxLength);
                b.Property(x => x.CVUrl).HasColumnName(nameof(JobApplication.CVUrl));
                b.Property(x => x.CoverLetterUrl).HasColumnName(nameof(JobApplication.CoverLetterUrl));
                b.Property(x => x.AdditionalDocumentUrl).HasColumnName(nameof(JobApplication.AdditionalDocumentUrl));
                b.Property(x => x.ResponseUrl).HasColumnName(nameof(JobApplication.ResponseUrl)).HasMaxLength(255);
                b.HasOne<Vacancy>().WithMany().IsRequired().HasForeignKey(x => x.VacancyId).OnDelete(DeleteBehavior.NoAction);
                b.HasMany(x => x.DiversityDatas).WithOne().HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.JobFormResponses).WithOne().HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasMany(x => x.DiversityFormResponses).WithOne().HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<DiversityFormResponse>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "DiversityFormResponses", RockITConsts.DiversityDbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FormStructureJson).HasColumnName(nameof(DiversityFormResponse.FormStructureJson)).IsRequired();
                b.Property(x => x.FormResponseJson).HasColumnName(nameof(DiversityFormResponse.FormResponseJson)).IsRequired();
                b.HasOne<JobApplication>().WithMany(x => x.DiversityFormResponses).HasForeignKey(x => x.JobApplicationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

        }
        if (builder.IsHostDatabase())
        { 
            builder.Entity<Vacancy>(b =>
            {
                b.Ignore(x => x.Regions);
                b.Ignore(x => x.Groups);
                b.ToTable(RockITConsts.DbTablePrefix + "Vacancies", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Title).HasColumnName(nameof(Vacancy.Title)).IsRequired().HasMaxLength(VacancyConsts.TitleMaxLength);
                b.Property(x => x.Reference).HasColumnName(nameof(Vacancy.Reference)).IsRequired().HasMaxLength(VacancyConsts.ReferenceMaxLength);
                b.Property(x => x.Role).HasColumnName(nameof(Vacancy.Role)).HasMaxLength(VacancyConsts.RoleMaxLength);
                b.Property(x => x.Benefits).HasColumnName(nameof(Vacancy.Benefits)).HasMaxLength(VacancyConsts.BenefitsMaxLength);
                b.Property(x => x.Location).HasColumnName(nameof(Vacancy.Location)).HasMaxLength(VacancyConsts.LocationMaxLength);
                b.Property(x => x.Salary).HasColumnName(nameof(Vacancy.Salary)).HasMaxLength(VacancyConsts.SalaryMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(Vacancy.Description)).IsRequired();
                b.Property(x => x.FormalInterviewDate).HasColumnName(nameof(Vacancy.FormalInterviewDate));
                b.Property(x => x.SecondInterviewDate).HasColumnName(nameof(Vacancy.SecondInterviewDate));
                b.Property(x => x.ExternalPostingDate).HasColumnName(nameof(Vacancy.ExternalPostingDate));
                b.Property(x => x.ClosingDate).HasColumnName(nameof(Vacancy.ClosingDate));
                b.Property(x => x.ExpiringDate).HasColumnName(nameof(Vacancy.ExpiringDate));
                b.Property(x => x.BrochureFileId).HasColumnName(nameof(Vacancy.BrochureFileId));
                b.Property(x => x.AdditionalFileId).HasColumnName(nameof(Vacancy.AdditionalFileId));
                b.Property(x => x.ShowDiversity).HasColumnName(nameof(Vacancy.ShowDiversity));
                b.Property(x => x.Flag_HideVacancy).HasColumnName(nameof(Vacancy.Flag_HideVacancy));
                b.HasOne<Company>().WithMany().IsRequired().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<IdentityUser>().WithMany().IsRequired().HasForeignKey(x => x.IdentityUserId).OnDelete(DeleteBehavior.NoAction);
               
                b.HasOne<FormDefinition>().WithMany().HasForeignKey(x => x.VacancyFormDefinitionId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<FormDefinition>().WithMany().HasForeignKey(x => x.DiversityFormDefinitionId).OnDelete(DeleteBehavior.NoAction);
                b.HasMany<VacancyRegion>("VacancyRegions").WithOne().HasForeignKey(x => x.VacancyId).IsRequired();
                b.HasMany<PracticeGroup>("PracticeGroups").WithMany().UsingEntity<VacancyPracticeGroup>();
                //b.HasMany(x => x.Contributors).WithOne().HasForeignKey(x => x.VacancyId).IsRequired();
            });

            builder.Entity<VacancyRegion>(b =>
            {
                b.HasKey(e => new { e.VacancyId,  e.Region});
                b.ToTable(RockITConsts.DbTablePrefix + "VacancyRegions", RockITConsts.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<VacancyPracticeGroup>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "VacancyPracticeGroups", RockITConsts.DbSchema);
                b.ConfigureByConvention();
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<BrochureSubscription>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "BrochureSubscriptions", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(BrochureSubscription.FirstName)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(BrochureSubscription.LastName)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.Property(x => x.EmailAddress).HasColumnName(nameof(BrochureSubscription.EmailAddress)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(BrochureSubscription.PhoneNumber)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.Property(x => x.CurrentRole).HasColumnName(nameof(BrochureSubscription.CurrentRole)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.Property(x => x.CurrentCompany).HasColumnName(nameof(BrochureSubscription.CurrentCompany)).IsRequired().HasMaxLength(BrochureSubscriptionConsts.DefaultMaxLength);
                b.HasOne<Vacancy>().WithMany().IsRequired().HasForeignKey(x => x.VacancyId).OnDelete(DeleteBehavior.NoAction);
            });

        }

        builder.Entity<AppFileDescriptors.AppFileDescriptor>(b =>
                    {
                        b.ToTable(RockITConsts.DbTablePrefix + "FileDescriptors", RockITConsts.DbSchema);
                        b.ConfigureByConvention();
                        b.Property(x => x.Name);
                        b.Property(x => x.MimeType);
                    });

        if (builder.IsHostDatabase())
        {
            builder.Entity<MediaSource>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "MediaSources", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(MediaSource.Name)).IsRequired().HasMaxLength(MediaSourceConsts.NameMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(MediaSource.Description)).IsRequired().HasMaxLength(MediaSourceConsts.DescriptionMaxLength);
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<VacancyMediaSource>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "VacancyMediaSources", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.VacancyId).HasColumnName(nameof(VacancyMediaSource.VacancyId)).IsRequired();
                b.Property(x => x.MediaSourceId).HasColumnName(nameof(VacancyMediaSource.MediaSourceId)).IsRequired();
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<RoleType>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "RoleTypes", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(RoleType.Name)).IsRequired().HasMaxLength(RoleTypeConsts.NameMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(RoleType.Description)).IsRequired().HasMaxLength(RoleTypeConsts.DescriptionMaxLength);

            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<VacancyRoleType>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "VacancyRoleTypes", RockITConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.VacancyId).HasColumnName(nameof(VacancyRoleType.VacancyId)).IsRequired();
                b.Property(x => x.RoleTypeId).HasColumnName(nameof(VacancyRoleType.RoleTypeId)).IsRequired();
            });
        }

        if (builder.IsHostDatabase())
        {
            builder.Entity<VacancyContributor>(b =>
            {
                b.ToTable(RockITConsts.DbTablePrefix + "VacancyContributors", RockITConsts.DbSchema);
                b.ConfigureByConvention();

                //define composite key
                b.HasKey(x => new { x.VacancyId, x.IdentityUserId });

                //many-to-many configuration
                //b.HasOne<Vacancy>().WithMany(x => x.Contributors).HasForeignKey(x => x.VacancyId).IsRequired();
                //b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.IdentityUserId).IsRequired();

                b.HasIndex(x => new { x.VacancyId, x.IdentityUserId });
            });
        }
    }
}