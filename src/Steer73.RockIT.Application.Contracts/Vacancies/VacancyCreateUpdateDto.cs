using Steer73.RockIT.Enums;
using Steer73.RockIT.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyCreateUpdateDto : VacancyFilesCreateUpdateDto
    {
        [DisplayName("Job Title")]
        [Required]
        [StringLength(VacancyConsts.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(VacancyConsts.ReferenceMaxLength)]
        public string Reference { get; set; } = null!;

        [DisplayName("Regions")]
        [RequiredEnumerable]
        public List<Region> Regions { get; set; } = [];

        [DisplayName("Role Level")]
        [StringLength(VacancyConsts.RoleMaxLength)]
        public string? Role { get; set; }

        [StringLength(VacancyConsts.BenefitsMaxLength)]
        public string? Benefits { get; set; }

        [Required]
        [StringLength(VacancyConsts.LocationMaxLength)]
        public string Location { get; set; } = null!;

        [Required]
        [StringLength(VacancyConsts.SalaryMaxLength)]
        public string Salary { get; set; } = null!;

        [StringLength(VacancyConsts.LinkedInUrlMaxLength)]
        public string? LinkedInUrl { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [DisplayName("Formal Interview Date")]
        public string? FormalInterviewDate { get; set; }

        [DisplayName("Second Interview Date")]
        public string? SecondInterviewDate { get; set; }

        [DisplayName("Company")]
        [Required]
        [RequiredNonDefault]
        public Guid? CompanyId { get; set; }

        [DisplayName("Role coordinated by")]
        [Required]
        [RequiredNonDefault]
        public Guid? IdentityUserId { get; set; }

        [DisplayName("Contributors")]
        public List<Guid>? ContributorIds { get; set; }

        [DisplayName("Sectors")]
        [Required]
        [RequiredEnumerable]
        public List<Guid> PracticeGroupIds { get; set; } = [];

        [DisplayName("Vacancy specific form")]
        public Guid? VacancyFormDefinitionId { get; set; }

        [DisplayName("Diversity form")]
        public Guid? DiversityFormDefinitionId { get; set; }

        [DisplayName("Media Sources")]
        public List<Guid>? MediaSourceIds { get; set; }

        [DisplayName("Role Types")]
        public List<Guid>? RoleTypeIds { get; set; }

        public bool ShowDiversity { get; set; }

        public bool Flag_HideVacancy { get; set; }



    }
}