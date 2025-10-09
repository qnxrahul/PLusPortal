using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationCreateDtoBase
    {
        [Required]
        [StringLength(JobApplicationConsts.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(JobApplicationConsts.LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(JobApplicationConsts.DefaultStringMaxLength)]
        public string Aka { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(JobApplicationConsts.EmailAddressMaxLength)]
        public string EmailAddress { get; set; } = null!;
        [StringLength(JobApplicationConsts.TitleMaxLength)]
        public string? Title { get; set; }
        [StringLength(JobApplicationConsts.PhoneNumberMaxLength)]
        public string? PhoneNumber { get; set; }
        [StringLength(JobApplicationConsts.LandlineMaxLength)]
        public string? Landline { get; set; }
        [StringLength(JobApplicationConsts.CurrentRoleMaxLength)]
        public string? CurrentRole { get; set; }
        [StringLength(JobApplicationConsts.CurrentCompanyMaxLength)]
        public string? CurrentCompany { get; set; }
        [StringLength(JobApplicationConsts.CurrentPositionTypeMaxLength)]
        public string? CurrentPositionType { get; set; }
        public string? CVUrl { get; set; }
        public string? CoverLetterUrl { get; set; }
        public string? AdditionalDocumentUrl { get; set; }
        public Guid VacancyId { get; set; }
    }
}