using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationManagerBase : DomainService
    {
        protected IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationManagerBase(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public virtual async Task<JobApplication> CreateAsync(
            Guid vacancyId, 
            string firstName, 
            string lastName, 
            string aka,
            string emailAddress, 
            JobApplicationStatus status, 
            SyncStatus syncStatus, 
            SyncStatus approveEmailSyncStatus, 
            SyncStatus rejectEmailSyncStatus, 
            string? title = null, 
            string? phoneNumber = null, 
            string? landline = null, 
            string? currentRole = null, 
            string? currentCompany = null, 
            string? currentPositionType = null, 
            string? cVUrl = null, 
            string? coverLetterUrl = null, 
            string? additionalDocumentUrl = null,
            string? responseUrl = null,
            DateTime? statusUpdate = null, 
            DateTime? syncStatusUpdate = null,
            DateTime? approveEmailUpdate = null,
            DateTime? rejectEmailUpdate = null)
        {
            Check.NotNull(vacancyId, nameof(vacancyId));
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.Length(firstName, nameof(firstName), JobApplicationConsts.FirstNameMaxLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            Check.Length(lastName, nameof(lastName), JobApplicationConsts.LastNameMaxLength);

            Check.NotNullOrWhiteSpace(aka, nameof(aka));
            Check.Length(aka, nameof(aka), JobApplicationConsts.DefaultStringMaxLength);

            Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Check.Length(emailAddress, nameof(emailAddress), JobApplicationConsts.EmailAddressMaxLength);
            Check.Length(title, nameof(title), JobApplicationConsts.TitleMaxLength);

            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Check.Length(phoneNumber, nameof(phoneNumber), JobApplicationConsts.PhoneNumberMaxLength);

            Check.Length(landline, nameof(landline), JobApplicationConsts.LandlineMaxLength);
            Check.Length(currentRole, nameof(currentRole), JobApplicationConsts.CurrentRoleMaxLength);
            Check.Length(currentCompany, nameof(currentCompany), JobApplicationConsts.CurrentCompanyMaxLength);
            Check.Length(currentPositionType, nameof(currentPositionType), JobApplicationConsts.CurrentPositionTypeMaxLength);

            var jobApplication = new JobApplication(
             GuidGenerator.Create(),
             vacancyId, 
             firstName, 
             lastName, 
             aka,
             emailAddress, 
             status, 
             syncStatus, 
             approveEmailSyncStatus, 
             rejectEmailSyncStatus, 
             title,
             phoneNumber, 
             landline, 
             currentRole, 
             currentCompany, 
             currentPositionType, 
             cVUrl, 
             coverLetterUrl, 
             additionalDocumentUrl,
             responseUrl,
             statusUpdate, 
             syncStatusUpdate, 
             approveEmailUpdate, 
             rejectEmailUpdate);

            return await _jobApplicationRepository.InsertAsync(jobApplication, autoSave: true);
        }

        public virtual async Task<JobApplication> UpdateAsync(
            Guid id,
            Guid vacancyId, string firstName, string lastName, string aka, string emailAddress, string? title = null, string? phoneNumber = null, string? landline = null, string? currentRole = null, string? currentCompany = null, string? currentPositionType = null, string? cVUrl = null, string? coverLetterUrl = null, string? additionalDocumentUrl = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNull(vacancyId, nameof(vacancyId));
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.Length(firstName, nameof(firstName), JobApplicationConsts.FirstNameMaxLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            Check.Length(lastName, nameof(lastName), JobApplicationConsts.LastNameMaxLength);

            Check.NotNullOrWhiteSpace(aka, nameof(aka));
            Check.Length(aka, nameof(aka), JobApplicationConsts.DefaultStringMaxLength);

            Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Check.Length(emailAddress, nameof(emailAddress), JobApplicationConsts.EmailAddressMaxLength);
            Check.Length(title, nameof(title), JobApplicationConsts.TitleMaxLength);

            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Check.Length(phoneNumber, nameof(phoneNumber), JobApplicationConsts.PhoneNumberMaxLength);

            Check.Length(landline, nameof(landline), JobApplicationConsts.LandlineMaxLength);
            Check.Length(currentRole, nameof(currentRole), JobApplicationConsts.CurrentRoleMaxLength);
            Check.Length(currentCompany, nameof(currentCompany), JobApplicationConsts.CurrentCompanyMaxLength);
            Check.Length(currentPositionType, nameof(currentPositionType), JobApplicationConsts.CurrentPositionTypeMaxLength);

            var jobApplication = await _jobApplicationRepository.GetAsync(id);

            jobApplication.VacancyId = vacancyId;
            jobApplication.FirstName = firstName;
            jobApplication.LastName = lastName;
            jobApplication.Aka = aka;
            jobApplication.EmailAddress = emailAddress;
            jobApplication.Title = title;
            jobApplication.PhoneNumber = phoneNumber;
            jobApplication.Landline = landline;
            jobApplication.CurrentRole = currentRole;
            jobApplication.CurrentCompany = currentCompany;
            jobApplication.CurrentPositionType = currentPositionType;
            jobApplication.CVUrl = cVUrl;
            jobApplication.CoverLetterUrl = coverLetterUrl;
            jobApplication.AdditionalDocumentUrl = additionalDocumentUrl;

            jobApplication.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _jobApplicationRepository.UpdateAsync(jobApplication);
        }

    }
}