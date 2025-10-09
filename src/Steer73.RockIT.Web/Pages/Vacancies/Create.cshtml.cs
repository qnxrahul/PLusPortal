using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Web.Pages.Vacancies;

public class CreateModel : RockITPageModel
{
    [BindProperty]
    public VacancyCreateInputModel Vacancy { get; set; }

    public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
    {
         new SelectListItem(" — ", "")
    };

    public List<SelectListItem> VacancyFormDefinitionLookupList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem(" — ", "")
    };

    public List<SelectListItem> DiversityFormDefinitionLookupList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem(" — ", "")
    };

    public List<PracticeGroupDto> SelectedPracticeGroups { get; set; } = [];
    public List<RoleTypeDto> SelectedRoleTypes { get; set; } = [];
    public List<MediaSourceDto> SelectedMediaSources { get; set; } = [];

    readonly IVacanciesAppService _vacanciesAppService;

    public CreateModel(IVacanciesAppService vacanciesAppService)
    {
        _vacanciesAppService = vacanciesAppService;
        Vacancy = new();
    }

    public virtual async Task OnGetAsync()
    {
        Vacancy = new VacancyCreateInputModel
        {
            Reference = await _vacanciesAppService.GetNewVacancyReferenceNumberAsync()
        };
        await Initialize();

        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await Initialize();
            return Page();
        }

        Vacancy.Description = Utilities.Helpers.SanitizeDescription(Vacancy.Description);
        await _vacanciesAppService.CreateAsync(ObjectMapper.Map<VacancyCreateInputModel, VacancyCreateDto>(Vacancy));
        return RedirectToPage("/Vacancies/Index");
    }

    private async Task Initialize()
    {       
        IdentityUserLookupListRequired.AddRange((                   
            await _vacanciesAppService.GetIdentityUserLookupAsync(new LookupRequestDto                   
            {                    
                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                
            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());

        var formDefinitions = await _vacanciesAppService.GetFormDefinitionLookupAsync(new LookupRequestDto
        {
            MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
        });

        VacancyFormDefinitionLookupList.AddRange(
            formDefinitions
                .Items.Where(x => x.FormType == RockIT.FormDefinitions.FormType.VacancyDiversityType)
                .Select(t => new SelectListItem(t.DisplayName, t.Id.ToString()) { Disabled = true }).ToList()
        );

        DiversityFormDefinitionLookupList.AddRange(
            formDefinitions
                .Items.Where(x => x.FormType == RockIT.FormDefinitions.FormType.DiversityType)
                .Select(t => new SelectListItem(t.DisplayName, t.Id.ToString()) { Disabled = true }).ToList()
        );

        if (Vacancy.MediaSourceIds is not null && Vacancy.MediaSourceIds.Count > 0)
        {
            SelectedMediaSources = await _vacanciesAppService.GetMediaSources(Vacancy.MediaSourceIds);
        }

        if (Vacancy.RoleTypeIds is not null && Vacancy.RoleTypeIds.Count > 0)
        {
            SelectedRoleTypes = await _vacanciesAppService.GetRoleTypes(Vacancy.RoleTypeIds);
        }

        if (Vacancy.PracticeGroupIds is not null && Vacancy.PracticeGroupIds.Count > 0)
        {
            SelectedPracticeGroups = await _vacanciesAppService.GetPracticeGroups(Vacancy.PracticeGroupIds);
        }
    }
}

public class VacancyCreateInputModel : VacancyCreateUpdateDto 
{
    public VacancyCreateInputModel()
    {
        ShowDiversity = true;
    }

    /// <summary>
    /// Abp DatePicker works with DateTime and doesn't work well with DateOnly
    /// </summary>
    [DisplayName("External Posting Date")]
    [Required]
    [RequiredNonDefault]
    [Before(otherProperty: nameof(ExpiringDate), otherPropertyDisplayName: "Expiring Date")]
    public DateTime? ExternalPostingDate { get; set; }

    [DisplayName("Closing Date")]
    [Required]
    [RequiredNonDefault]
    public DateTime? ClosingDate { get; set; }

    [DisplayName("Expiring Date")]
    [Required]
    [RequiredNonDefault]
    [Before(otherProperty: nameof(ClosingDate), otherPropertyDisplayName: "Closing Date")]
    public DateTime? ExpiringDate { get; set; }

    [DisplayName("EzekiaID")]
	[Required]
	public int? ExternalRefId { get; set; }
}