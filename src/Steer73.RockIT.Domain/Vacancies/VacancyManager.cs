using JetBrains.Annotations;
using Steer73.RockIT.Enums;
using Steer73.RockIT.PracticeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyManagerBase : DomainService
    {
        protected IVacancyRepository _vacancyRepository;
        protected IPracticeGroupRepository _practiceGroupRepository;
        protected IRepository<IdentityUser, Guid> _identityUserRepository;

        public VacancyManagerBase(
            IVacancyRepository vacancyRepository,
            IPracticeGroupRepository practiceGroupRepository,
            IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _vacancyRepository = vacancyRepository;
            _practiceGroupRepository = practiceGroupRepository;
            _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<Vacancy> CreateAsync(
        Guid companyId, Guid identityUserId, List<Guid> practiceGroupIds, Guid? vacancyFormDefinitionId, Guid? diversityFormDefinitionId, string title, string reference, IReadOnlyCollection<Region> regions, List<Guid> contributorIds, string description, DateOnly externalPostingDate, DateOnly closingDate, DateOnly expiringDate, bool showDiversity,bool flagHideVacancy, string? role = null, string? benefits = null, string? location = null, string? salary = null, string? formalInterviewDate = null, string? secondInterviewDate = null, Guid? brochureFileId = null, Guid? additionalFileId = null, string? linkedInUrl = null,
        int? externalRefId = null)
        {
            Check.NotNull(companyId, nameof(companyId));
            Check.NotNull(identityUserId, nameof(identityUserId));
            Check.NotNullOrWhiteSpace(title, nameof(title));
            Check.Length(title, nameof(title), VacancyConsts.TitleMaxLength);
            Check.NotNullOrWhiteSpace(reference, nameof(reference));
            Check.Length(reference, nameof(reference), VacancyConsts.ReferenceMaxLength);
            Check.NotNullOrWhiteSpace(description, nameof(description));
            Check.Length(role, nameof(role), VacancyConsts.RoleMaxLength);
            Check.Length(benefits, nameof(benefits), VacancyConsts.BenefitsMaxLength);

            Check.NotNullOrWhiteSpace(location, nameof(location));
            Check.Length(location, nameof(location), VacancyConsts.LocationMaxLength);

            Check.NotNullOrWhiteSpace(salary, nameof(salary));
            Check.Length(salary, nameof(salary), VacancyConsts.SalaryMaxLength);

            DateTime? brochureLastUpdatedAt = brochureFileId is null ? null : Clock.Now;

            var guid = GuidGenerator.Create();
            var practiceGroups = await _practiceGroupRepository.GetListAsync(x => practiceGroupIds.Contains(x.Id));
            var contributors = await _identityUserRepository.GetListAsync(x => contributorIds.Contains(x.Id));
        
            var vacancyContributors = contributors.Select(contributor => new VacancyContributor(guid, contributor.Id)).ToList();

            var vacancy = new Vacancy(
             guid,
             companyId, identityUserId, practiceGroups, vacancyFormDefinitionId, diversityFormDefinitionId, title, reference, regions, vacancyContributors, description, externalPostingDate, closingDate, expiringDate, showDiversity,flagHideVacancy, role, benefits, location, salary, formalInterviewDate, secondInterviewDate, brochureFileId, additionalFileId, linkedInUrl,
             brochureLastUpdatedAt: brochureLastUpdatedAt,
             externalRefId: externalRefId);
          
            return await _vacancyRepository.InsertAsync(vacancy);
        }

        public virtual async Task<Vacancy> UpdateAsync(
            Guid id,
            Guid companyId, Guid identityUserId, List<Guid> practiceGroupIds, Guid? vacancyFormDefinitionId, Guid? diversityFormDefinitionId, string title, string reference, IReadOnlyCollection<Region> regions, List<Guid> contributorIds, string description, DateOnly externalPostingDate, DateOnly closingDate, DateOnly expiringDate, bool showDiversity, bool flagHideVacancy, string? role = null, string? benefits = null, string? location = null, string? salary = null, string? formalInterviewDate = null, string? secondInterviewDate = null, Guid? brochureFileId = null, Guid? additionalFileId = null, string? linkedInUrl = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(companyId, nameof(companyId));
            Check.NotNull(identityUserId, nameof(identityUserId));
            Check.NotNullOrWhiteSpace(title, nameof(title));
            Check.Length(title, nameof(title), VacancyConsts.TitleMaxLength);
            Check.NotNullOrWhiteSpace(reference, nameof(reference));
            Check.Length(reference, nameof(reference), VacancyConsts.ReferenceMaxLength);
            Check.NotNullOrWhiteSpace(description, nameof(description));
            Check.Length(role, nameof(role), VacancyConsts.RoleMaxLength);
            Check.Length(benefits, nameof(benefits), VacancyConsts.BenefitsMaxLength);

            Check.NotNullOrWhiteSpace(location, nameof(location));
            Check.Length(location, nameof(location), VacancyConsts.LocationMaxLength);

            Check.NotNullOrWhiteSpace(salary, nameof(salary));
            Check.Length(salary, nameof(salary), VacancyConsts.SalaryMaxLength);

            var vacancy = await _vacancyRepository.GetAsync(id);

            vacancy.CompanyId = companyId;
            vacancy.IdentityUserId = identityUserId;
            vacancy.VacancyFormDefinitionId = vacancyFormDefinitionId;
            vacancy.DiversityFormDefinitionId = diversityFormDefinitionId;
            vacancy.Title = title;
            vacancy.Reference = reference;
            vacancy.Description = description;
            vacancy.ExternalPostingDate = externalPostingDate;
            vacancy.ClosingDate = closingDate;
            vacancy.ExpiringDate = expiringDate;
            vacancy.ShowDiversity = showDiversity;
            vacancy.Flag_HideVacancy = flagHideVacancy;
            vacancy.Role = role;
            vacancy.Benefits = benefits;
            vacancy.Location = location;
            vacancy.Salary = salary;
            vacancy.FormalInterviewDate = formalInterviewDate;
            vacancy.SecondInterviewDate = secondInterviewDate;

            if (vacancy.BrochureFileId != brochureFileId)
            {
                vacancy.BrochureFileId = brochureFileId;
                vacancy.BrochureLastUpdatedAt = Clock.Now;
            }

            vacancy.AdditionalFileId = additionalFileId;
            vacancy.LinkedInUrl = linkedInUrl;
            
            vacancy.SetRegions(regions);

            var practiceGroups = await _practiceGroupRepository.GetListAsync(x => practiceGroupIds.Contains(x.Id));
            vacancy.SetPracticeGroups(practiceGroups);

            var contributors = await _identityUserRepository.GetListAsync(x => contributorIds.Contains(x.Id));
            var vacancyContributors = contributors.Select(contributor => new VacancyContributor(vacancy.Id, contributor.Id)).ToList();
            vacancy.SetContributors(vacancyContributors);

            vacancy.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _vacancyRepository.UpdateAsync(vacancy);
        }
    }
}