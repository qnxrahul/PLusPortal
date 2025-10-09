using JetBrains.Annotations;
using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.JobApplications
{
    public class NewJobApplicationComplete
    {
        [NotNull]
        public virtual string FirstName { get; set; }

        [NotNull]
        public virtual string LastName { get; set; }

        [NotNull]
        public virtual string Aka { get; set; }

        [NotNull]
        public virtual string EmailAddress { get; set; }

        [CanBeNull]
        public virtual string? Title { get; set; }

        [CanBeNull]
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

        public Guid VacancyId { get; set; }

        public virtual YesNo? Diversity_HappyToCompleteForm { get; set; }

        public virtual AgeRange? Diversity_AgeRange { get; set; }

        public virtual GenderOrSex? Diversity_Gender { get; set; }

        [CanBeNull]
        public virtual string? Diversity_OtherGender { get; set; }

        public virtual YesNoPreferNotToSay? Diversity_GenderIdentitySameAsBirth { get; set; }

        public virtual GenderOrSex? Diversity_Sex { get; set; }

        [CanBeNull]
        public virtual string? Diversity_OtherSex { get; set; }

        public virtual SexualOrientation? Diversity_SexualOrientation { get; set; }

        [CanBeNull]
        public virtual string? Diversity_OtherSexualOrientation { get; set; }

        public virtual Ethnicity? Diversity_Ethnicity { get; set; }

        [CanBeNull]
        public virtual string? Diversity_OtherEthnicity { get; set; }

        public virtual Religion? Diversity_ReligionOrBelief { get; set; }

        [CanBeNull]
        public virtual string? Diversity_OtherReligionOrBelief { get; set; }

        public virtual YesNoPreferNotToSay? Diversity_Disability { get; set; }

        public virtual EducationLevel? Diversity_EducationLevel { get; set; }

        public virtual TypeOfSecondarySchool? Diversity_TypeOfSecondarySchool { get; set; }
        [CanBeNull]
        public virtual string? Diversity_OtherTypeOfSecondarySchool { get; set; }

        public virtual YesNoPreferNotToSayDontKnow? Diversity_HigherEducationQualifications { get; set; }

        public string? JobFormResponse { get; set; }
        public string? DiversityFormResponse { get; set; }
	}
}
