using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Steer73.RockIT.ApplicantPortal;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Steer73.RockIT.Web.Pages
{
    public class VacancyFormModel : PageModel
    {
        private readonly IApplicantPortalAppService _applicantPortalAppService;
        private static readonly Guid RegistrationTemplateVacancyId = Guid.Parse("77560497-e1a3-23bd-6399-3a1c2ca9a1ff");

        public Guid VacancyId { get; set; }
        public VacancyWithNavigationPropertiesDto VacancyDto { get; set; } = new VacancyWithNavigationPropertiesDto();
        public bool IsRegistrationVacancy { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string FormDefinitionDetails { get; set; }

        [Required]
        [StringLength(JobApplicationConsts.FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(JobApplicationConsts.LastNameMaxLength)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(JobApplicationConsts.EmailAddressMaxLength)]
        public string? EmailAddress { get; set; }

        [StringLength(JobApplicationConsts.TitleMaxLength)]
        public string? Title { get; set; }

        [Display(Name = "Preferred Name")]
        [Required]
        [StringLength(JobApplicationConsts.DefaultStringMaxLength)]
        public string Aka { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Phone number must be at least 8 digits.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Phone number must contain only digits.")]
        [StringLength(JobApplicationConsts.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [CanBeNull]
        [MinLength(8, ErrorMessage = "Landline must be at least 8 digits.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Landline must contain only digits.")]
        [StringLength(JobApplicationConsts.LandlineMaxLength)]
        public string? Landline { get; set; }

        [Required]
        [StringLength(JobApplicationConsts.CurrentRoleMaxLength)]
        public string CurrentRole { get; set; }

        [Display(Name = "Current Organisation")]
        [Required]
        [StringLength(JobApplicationConsts.CurrentCompanyMaxLength)]
        public string CurrentCompany { get; set; }

        [Required]
        [StringLength(JobApplicationConsts.CurrentPositionTypeMaxLength)]
        public string CurrentPositionType { get; set; }

        [Display(Name = "CV")]
        [Required]
        public IFormFile? FileCv { get; set; }

        [Display(Name = "Cover Letter")]
        [Required]
        public IFormFile FileCoverLetter { get; set; }

        [Display(Name = "Additional Doc")]
        public IFormFile? FileAdditionalDoc { get; set; }

        public List<SelectListItem> PositionTypeList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Permanent", Text = "Permanent"},
            new SelectListItem { Value = "Temporary", Text = "Temporary"},
            new SelectListItem { Value = "Part", Text = "Part" },
            new SelectListItem { Value = "Interim", Text = "Interim" },
            new SelectListItem { Value = "Contract", Text = "Contract" },
            new SelectListItem { Value = "NED", Text = "NED" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

        public VacancyFormModel(
                  IApplicantPortalAppService applicantPortalAppService
                    )
        {
            _applicantPortalAppService = applicantPortalAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid vacancyId)
        {
            VacancyId = vacancyId;
            VacancyDto = await _applicantPortalAppService.GetVacancy(vacancyId);
            IsRegistrationVacancy = VacancyDto?.Vacancy?.Id == RegistrationTemplateVacancyId;
            //FormDefinitionDetails = VacancyDto.FormDefinition.FormDetails ?? "";
            FormDefinitionDetails = "";
            ModelState.Clear();
            return Page();
        }
    }
}
