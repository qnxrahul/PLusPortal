using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.Enums;
using Steer73.RockIT.ValidationAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.JobApplications
{
    public class NewJobApplicationCompleteBaseDto
    {
        [Required]
        [StringLength(JobApplicationConsts.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(JobApplicationConsts.LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Display(Name = "Preferred Name")]
        [Required]
        [StringLength(JobApplicationConsts.DefaultStringMaxLength)]
        public string Aka { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(JobApplicationConsts.EmailAddressMaxLength)]
        public string EmailAddress { get; set; } = null!;

        [StringLength(JobApplicationConsts.TitleMaxLength)]
        public string? Title { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Phone number must be at least 8 digits.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Phone number must contain only digits.")]
        [StringLength(JobApplicationConsts.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [MinLength(8, ErrorMessage = "Landline must be at least 8 digits.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Landline must contain only digits.")]
        [StringLength(JobApplicationConsts.LandlineMaxLength)]
        public string? Landline { get; set; }

        [Required]
        [StringLength(JobApplicationConsts.CurrentRoleMaxLength)]
        public string CurrentRole { get; set; } = null!;

        [Display(Name = "Current Organisation")]
        [Required]
        [StringLength(JobApplicationConsts.CurrentCompanyMaxLength)]
        public string CurrentCompany { get; set; } = null!;

        [StringLength(JobApplicationConsts.CurrentPositionTypeMaxLength)]
        public string CurrentPositionType { get; set; } = null!;

        public Guid VacancyId { get; set; }

        public YesNo? Diversity_HappyToCompleteForm { get; set; }

        public AgeRange? Diversity_AgeRange { get; set; }

        public GenderOrSex? Diversity_Gender { get; set; }

        [StringLength(DiversityDataConsts.OtherGenderMaxLength)]
        public string? Diversity_OtherGender { get; set; }

        public YesNoPreferNotToSay? Diversity_GenderIdentitySameAsBirth { get; set; }

        public GenderOrSex? Diversity_Sex { get; set; }

        [StringLength(DiversityDataConsts.OtherSexMaxLength)]
        public string? Diversity_OtherSex { get; set; }

        public SexualOrientation? Diversity_SexualOrientation { get; set; }

        [StringLength(DiversityDataConsts.OtherSexualOrientationMaxLength)]
        public string? Diversity_OtherSexualOrientation { get; set; }

        public Ethnicity? Diversity_Ethnicity { get; set; }

        [StringLength(DiversityDataConsts.OtherEthnicityMaxLength)]
        public string? Diversity_OtherEthnicity { get; set; }

        public Religion? Diversity_ReligionOrBelief { get; set; }

        [StringLength(DiversityDataConsts.OtherReligionOrBeliefMaxLength)]
        public string? Diversity_OtherReligionOrBelief { get; set; }

        public YesNoPreferNotToSay? Diversity_Disability { get; set; }

        public EducationLevel? Diversity_EducationLevel { get; set; }

        public TypeOfSecondarySchool? Diversity_TypeOfSecondarySchool { get; set; }

        [StringLength(DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength)]
        public string? Diversity_OtherTypeOfSecondarySchool { get; set; }

        public YesNoPreferNotToSayDontKnow? Diversity_HigherEducationQualifications { get; set; }

        public string? JobFormResponse { get; set; }

        public string? DiversityFormResponse { get; set; }
    }

    public class NewJobApplicationCompleteDto : NewJobApplicationCompleteBaseDto
    {
        [DisplayName("CV")]
        [AllowedFileExtensions([".pdf", ".docx"])]
        [MaxFileSize(JobApplicationConsts.MaxFileSize)]
        [Required]
        public BlobDto FileCv { get; set; } = null!;

        [DisplayName("Cover Letter")]
        [AllowedFileExtensions([".pdf", ".docx"])]
        [MaxFileSize(JobApplicationConsts.MaxFileSize)]
        [Required]
        public BlobDto FileCoverLetter { get; set; } = null!;

        [DisplayName("Additional Doc")]
        [AllowedFileExtensions([".pdf", ".docx"])]
        [MaxFileSize(JobApplicationConsts.MaxFileSize)]
        public BlobDto? FileAdditionalDoc { get; set; }
    }
}
