using JetBrains.Annotations;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.JobFormResponses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string FirstName { get; set; }

        [NotNull]
        public virtual string LastName { get; set; }

        [CanBeNull]
        public virtual string? Aka { get; set; }

        [NotNull]
        public virtual string EmailAddress { get; set; }

        [CanBeNull]
        public virtual string? Title { get; set; }

        public virtual string? PhoneNumber { get; set; }

        [CanBeNull]
        public virtual string? Landline { get; set; }

        [CanBeNull]
        public virtual string? CurrentRole { get; set; }

        [CanBeNull]
        public virtual string? CurrentCompany { get; set; }

        [CanBeNull]
        public virtual string? CurrentPositionType { get; set; }

        public virtual string? CVUrl { get; set; }

        public virtual string? CoverLetterUrl { get; set; }

        public virtual string? AdditionalDocumentUrl { get; set; }

        public virtual string? ResponseUrl { get; set; }

        [CanBeNull]
        public DateTime? BrochureSentAt { get; set; }

        public Guid VacancyId { get; set; }
        public ICollection<DiversityData> DiversityDatas { get; private set; }
        public ICollection<JobFormResponse> JobFormResponses { get; private set; }
        public ICollection<DiversityFormResponse> DiversityFormResponses { get; private set; }

        protected JobApplicationBase()
        {

        }

        public JobApplicationBase(
            Guid id, 
            Guid vacancyId, 
            string firstName, 
            string lastName,
            string aka,
            string emailAddress, 
            string? title = null, 
            string? phoneNumber = null, 
            string? landline = null, 
            string? currentRole = null, 
            string? currentCompany = null, 
            string? currentPositionType = null, 
            string? cVUrl = null, 
            string? coverLetterUrl = null, 
            string? additionalDocumentUrl = null,
            string? responseUrl = null)
        {
            Id = id;
            Check.NotNull(firstName, nameof(firstName));
            Check.Length(firstName, nameof(firstName), JobApplicationConsts.FirstNameMaxLength, 0);
            Check.NotNull(lastName, nameof(lastName));
            Check.Length(lastName, nameof(lastName), JobApplicationConsts.LastNameMaxLength, 0);
            Check.NotNull(aka, nameof(aka));
            Check.Length(aka, nameof(aka), JobApplicationConsts.DefaultStringMaxLength, 0);
            Check.NotNull(emailAddress, nameof(emailAddress));
            Check.Length(emailAddress, nameof(emailAddress), JobApplicationConsts.EmailAddressMaxLength, 0);
            Check.Length(title, nameof(title), JobApplicationConsts.TitleMaxLength, 0);
            Check.NotNull(phoneNumber, nameof(phoneNumber));
            Check.Length(phoneNumber, nameof(phoneNumber), JobApplicationConsts.PhoneNumberMaxLength, 0);
            Check.Length(landline, nameof(landline), JobApplicationConsts.LandlineMaxLength, 0);
            Check.Length(currentRole, nameof(currentRole), JobApplicationConsts.CurrentRoleMaxLength, 0);
            Check.Length(currentCompany, nameof(currentCompany), JobApplicationConsts.CurrentCompanyMaxLength, 0);
            Check.Length(currentPositionType, nameof(currentPositionType), JobApplicationConsts.CurrentPositionTypeMaxLength, 0);
            FirstName = firstName;
            LastName = lastName;
            Aka = aka;
            EmailAddress = emailAddress;
            Title = title;
            PhoneNumber = phoneNumber;
            Landline = landline;
            CurrentRole = currentRole;
            CurrentCompany = currentCompany;
            CurrentPositionType = currentPositionType;
            CVUrl = cVUrl;
            CoverLetterUrl = coverLetterUrl;
            AdditionalDocumentUrl = additionalDocumentUrl;
            ResponseUrl = responseUrl;
            VacancyId = vacancyId;
            DiversityDatas = new Collection<DiversityData>();
            JobFormResponses = new Collection<JobFormResponse>();
            DiversityFormResponses = new Collection<DiversityFormResponse>();

        }
    }
}