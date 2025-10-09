using JetBrains.Annotations;
using Steer73.RockIT.Enums;
using Steer73.RockIT.PracticeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using static Steer73.RockIT.Permissions.RockITSharedPermissions;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Title { get; set; }

        [NotNull]
        public virtual string Reference { get; set; }

        [NotNull]
        protected virtual List<VacancyRegion> VacancyRegions { get; private set; } = [];

        public virtual IReadOnlyCollection<Region> Regions => VacancyRegions.Select(vr => vr.Region).ToList();

        [CanBeNull]
        public virtual string? Role { get; set; }

        [CanBeNull]
        public virtual string? Benefits { get; set; }

        [CanBeNull]
        public virtual string? Location { get; set; }

        [CanBeNull]
        public virtual string? Salary { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        public virtual bool Flag_HideVacancy { get; set; }

        public virtual string? FormalInterviewDate { get; set; }

        public virtual string? SecondInterviewDate { get; set; }

        public virtual DateOnly ExternalPostingDate { get; set; }

        public virtual DateOnly ClosingDate { get; set; }

        public virtual DateOnly ExpiringDate { get; set; }

        public virtual Guid? BrochureFileId { get; set; }

        public virtual DateTime? BrochureLastUpdatedAt { get; set; }

        public virtual Guid? AdditionalFileId { get; set; }

        public virtual bool ShowDiversity { get; set; }
        public Guid CompanyId { get; set; }
        public Guid IdentityUserId { get; set; }

        [NotNull]
        protected virtual List<PracticeGroup> PracticeGroups { get; private set; } = [];

        public virtual IReadOnlyCollection<PracticeGroup> Groups => PracticeGroups;

        public virtual ICollection<VacancyContributor> Contributors { get; private set; } = [];

        public Guid? VacancyFormDefinitionId { get; set; }
        public Guid? DiversityFormDefinitionId { get; set; }

        [CanBeNull]
        public virtual string? LinkedInUrl { get; set; }

        protected VacancyBase()
        {

        }

        public VacancyBase(Guid id, Guid companyId, Guid identityUserId, IReadOnlyCollection<PracticeGroup> practiceGroups, Guid? vacancyFormDefinitionId, Guid? diversityFormDefinitionId, string title, string reference, IReadOnlyCollection<Region> regions, ICollection<VacancyContributor> contributors, string description, DateOnly externalPostingDate, DateOnly closingDate, DateOnly expiringDate, bool showDiversity, bool flagHideVacancy, string? role = null, string? benefits = null, string? location = null, string? salary = null, string? formalInterviewDate = null, string? secondInterviewDate = null, Guid? brochureFileId = null, Guid? additionalFileId = null, string? linkedInUrl = null,
            DateTime? brochureLastUpdatedAt = null)
        {

            Id = id;
            Check.NotNull(title, nameof(title));
            Check.Length(title, nameof(title), VacancyConsts.TitleMaxLength, 0);
            Check.NotNull(reference, nameof(reference));
            Check.Length(reference, nameof(reference), VacancyConsts.ReferenceMaxLength, 0);
            Check.NotNull(regions, nameof(regions));
            Check.NotNull(description, nameof(description));
            Check.Length(role, nameof(role), VacancyConsts.RoleMaxLength, 0);
            Check.Length(benefits, nameof(benefits), VacancyConsts.BenefitsMaxLength, 0);
            Check.Length(location, nameof(location), VacancyConsts.LocationMaxLength, 0);
            Check.Length(salary, nameof(salary), VacancyConsts.SalaryMaxLength, 0);
            Title = title;
            Reference = reference;
            Description = description;
            ExternalPostingDate = externalPostingDate;
            ClosingDate = closingDate;
            ExpiringDate = expiringDate;
            ShowDiversity = showDiversity;
            Flag_HideVacancy = flagHideVacancy;
            Role = role;
            Benefits = benefits;
            Location = location;
            Salary = salary;
            FormalInterviewDate = formalInterviewDate;
            SecondInterviewDate = secondInterviewDate;
            BrochureFileId = brochureFileId;
            AdditionalFileId = additionalFileId;
            CompanyId = companyId;
            IdentityUserId = identityUserId;
            VacancyFormDefinitionId = vacancyFormDefinitionId;
            DiversityFormDefinitionId = diversityFormDefinitionId;
            LinkedInUrl = linkedInUrl;
            BrochureLastUpdatedAt = brochureLastUpdatedAt;

            SetContributors(contributors);
            SetRegions(regions);
            SetPracticeGroups(practiceGroups);
        }

        public void SetContributors(ICollection<VacancyContributor> contributors)
        {
            Contributors.Clear();
            Contributors = [.. contributors.DistinctBy(p => p.IdentityUserId).ToList()];
        }

        public void SetRegions(IReadOnlyCollection<Region> regions)
        {
            VacancyRegions.Clear();
            VacancyRegions = regions
                .Distinct()
                .Select(r => new VacancyRegion(Id, r))
                .ToList();    
        }

        public void SetPracticeGroups(IReadOnlyCollection<PracticeGroup> practiceGroups)
        {
            PracticeGroups.Clear();
            PracticeGroups = [.. practiceGroups.DistinctBy(p => p.Id).ToList()];
        }

        public void AddContributor(Guid contributorId)
        {
            Check.NotNull(contributorId, nameof(contributorId));

            if (IsInContributor(contributorId))
            {
                return;
            }

            Contributors.Add(new VacancyContributor(Id, contributorId));
        }

        public void RemoveContributor(Guid contributorId)
        {
            Check.NotNull(contributorId, nameof(contributorId));

            if (!IsInContributor(contributorId))
            {
                return;
            }

            Contributors.RemoveAll(x => x.IdentityUserId == contributorId);
        }
        private bool IsInContributor(Guid contributorId)
        {
            return Contributors.Any(x => x.IdentityUserId == contributorId);
        }
        public void RemoveAllContributors()
        {
            Contributors.RemoveAll(x => x.VacancyId == Id);
        }
        public void RemoveAllCategoriesExceptGivenIds(List<Guid> contributorIds)
        {
            Check.NotNullOrEmpty(contributorIds, nameof(contributorIds));

            Contributors.RemoveAll(x => !contributorIds.Contains(x.IdentityUserId));
        }
    }
}