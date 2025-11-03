using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Enums;
using Steer73.RockIT.FormDefinitions;
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
using Volo.Abp.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Steer73.RockIT.Web.Pages.Vacancies;

public class EditModel : RockITPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public VacancyUpdateInputModel Input { get; set; } = new();

    // for displaying the last time brochure file was updated if vacancy has a brochure file
    public DateTime? BrochureLastUpdatedAt { get; set; }

    public List<SelectListItem> IdentityUserLookupListRequired { get; set; } = new List<SelectListItem>
    {
    };

    public List<SelectListItem> PracticeGroupLookupListRequired { get; set; } = new List<SelectListItem>
    {
    };

    public List<SelectListItem> VacancyFormDefinitionLookupList { get; set; } = new List<SelectListItem>    
    {
        new SelectListItem(" � ", "")
    };

    public List<SelectListItem> DiversityFormDefinitionLookupList { get; set; } = new List<SelectListItem>    
    {    
        new SelectListItem(" � ", "")    
    };

    public List<SelectListItem> RegionLookupListRequired { get; set; } = new List<SelectListItem>    
    {    
        new SelectListItem(" � ", "")    
    };

    public List<SelectListItem> SelectedRoleTypes { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> SelectedMediaSources { get; set; } = new List<SelectListItem>();

    public List<PracticeGroupDto> SelectedPracticeGroups { get; set; } = [];
    public List<VacancyContributorDto> SelectedContributors { get; set; } = [];
    public CompanyDto? SelectedCompany { get; set; }

    protected IVacanciesAppService _vacanciesAppService;
    protected IFormDefinitionsAppService _formDefinitionsAppService;
    public string VacancyDetailUrl { get; set; } = string.Empty;
    private readonly IConfiguration _configuration;

    public EditModel(
        IVacanciesAppService vacanciesAppService,
        IFormDefinitionsAppService formDefinitionsAppService,
        IConfiguration configuration)
    {
        _vacanciesAppService = vacanciesAppService;
        _formDefinitionsAppService = formDefinitionsAppService;
        _configuration = configuration;
    }

    public virtual async Task OnGetAsync()
    {
        await Initialize();
    }
   
    public virtual async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await Initialize();
            return Page();
        }

        Input.Description = Utilities.Helpers.SanitizeDescription(Input.Description);
        var dto = ObjectMapper.Map<VacancyUpdateInputModel, VacancyUpdateDto>(Input);
        await _vacanciesAppService.UpdateAsync(Id, dto);
        return RedirectToPage("/Vacancies/Index");
    }

    private async Task Initialize()
    {
        var vacancyWithNavigationPropertiesDto = await _vacanciesAppService.GetWithNavigationPropertiesAsync(Id);
        Input = ObjectMapper.Map<VacancyDto, VacancyUpdateInputModel>(vacancyWithNavigationPropertiesDto.Vacancy);
        BrochureLastUpdatedAt = vacancyWithNavigationPropertiesDto.Vacancy.BrochureLastUpdatedAt;

		var vacancyFormDefinitions = await _formDefinitionsAppService.GetListAsync(new GetFormDefinitionsInput { CompanyId = vacancyWithNavigationPropertiesDto.Company.Id });
        var allowedFromDefinitions = vacancyFormDefinitions.Items.Select(x => x.FormDefinition.Id).ToArray();
        // Build public vacancy detail URL for display and copy
        var baseUrl = _configuration["App1:PortalBaseUrl"]?.TrimEnd('/');
        VacancyDetailUrl = string.IsNullOrWhiteSpace(baseUrl)
            ? $"/VacancyDetail/{Id}"
            : $"{baseUrl}/VacancyDetail/{Id}";

        IdentityUserLookupListRequired.AddRange((
            await _vacanciesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
            {
                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());

        PracticeGroupLookupListRequired.AddRange((
            await _vacanciesAppService.GetPracticeGroupLookupAsync(new LookupRequestDto
            {
                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount,
                IsActive = true
            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList());

        var formDefinitions = await _vacanciesAppService.GetFormDefinitionLookupAsync(new LookupRequestDto
        {
            MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
        });

        VacancyFormDefinitionLookupList.AddRange(
            formDefinitions
                .Items.Where(x => x.FormType == RockIT.FormDefinitions.FormType.VacancyDiversityType)
                .Select(t => new SelectListItem(t.DisplayName, t.Id.ToString()) { Disabled = !allowedFromDefinitions.Contains(t.Id) }).ToList()
        );

        DiversityFormDefinitionLookupList.AddRange(
            formDefinitions
                .Items.Where(x => x.FormType == RockIT.FormDefinitions.FormType.DiversityType)
                .Select(t => new SelectListItem(t.DisplayName, t.Id.ToString()) { Disabled = !allowedFromDefinitions.Contains(t.Id) }).ToList()
        );

        var regions = (Region[])Enum.GetValues(typeof(Region));
        RegionLookupListRequired.AddRange(regions.Select(x => new SelectListItem(x.GetDescription(), ((int)x).ToString())));

        var mediaSources = (await _vacanciesAppService.GetListOfMediaSourcesAsync()).Items;
        var selectedMediaSources = await _vacanciesAppService.GetListOfVacancyMediaSourcesAsync(Id);
        SelectedMediaSources.AddRange(selectedMediaSources.Select(x => new SelectListItem(mediaSources.FirstOrDefault(ms => ms.Id == x.MediaSourceId)?.Name, x.MediaSourceId.ToString())));

        var roleTypes = (await _vacanciesAppService.GetListOfRoleTypesAsync()).Items;
        var selectRoleTypes = await _vacanciesAppService.GetListOfVacancyRoleTypesAsync(Id);
        SelectedRoleTypes.AddRange(selectRoleTypes.Select(x => new SelectListItem(roleTypes.FirstOrDefault(rt => rt.Id == x.RoleTypeId)?.Name, x.RoleTypeId.ToString())));
        SelectedPracticeGroups = vacancyWithNavigationPropertiesDto.Vacancy.Groups;
        SelectedContributors = vacancyWithNavigationPropertiesDto.Vacancy.Contributors;
        SelectedCompany = vacancyWithNavigationPropertiesDto.Company;
        ViewData["EzekiaIDDisplayName"] = vacancyWithNavigationPropertiesDto.Vacancy.ProjectId ?? string.Empty;
    }
}

public class VacancyUpdateInputModel : VacancyCreateUpdateDto, IHasConcurrencyStamp
{
    /// <summary>
    /// Abp DatePicker works with DateTime and doesn't work well with DateOnly
    /// </summary>
    [DisplayName("Closing Date")]
    [Required]
    [RequiredNonDefault]
    public DateTime ClosingDate { get; set; }

    [DisplayName("Expiring Date")]
    [Required]
    [RequiredNonDefault]
    [Before(otherProperty: nameof(ClosingDate), otherPropertyDisplayName: "Closing Date")]
    public DateTime ExpiringDate { get; set; }

    [DisplayName("External Posting Date")]
    [Required]
    [RequiredNonDefault]
    [Before(otherProperty: nameof(ExpiringDate), otherPropertyDisplayName: "Expiring Date")]
    public DateTime ExternalPostingDate { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
