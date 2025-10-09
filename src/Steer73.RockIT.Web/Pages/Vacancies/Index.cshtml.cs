using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Enums;
using System.ComponentModel;

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? TitleFilter { get; set; }
        public string? ReferenceFilter { get; set; }
        public string? RegionFilter { get; set; }
        public string? RoleFilter { get; set; }
        public string? BenefitsFilter { get; set; }
        public string? LocationFilter { get; set; }
        public string? SalaryFilter { get; set; }
        public string? RoleTypeFilter { get; set; }
        public string? DescriptionFilter { get; set; }
        public DateOnly? FormalInterviewDateFilterMin { get; set; }

        public DateOnly? FormalInterviewDateFilterMax { get; set; }
        public DateOnly? SecondInterviewDateFilterMin { get; set; }

        public DateOnly? SecondInterviewDateFilterMax { get; set; }
        public DateOnly? ExternalPostingDateFilterMin { get; set; }

        public DateOnly? ExternalPostingDateFilterMax { get; set; }
        public DateOnly? ClosingDateFilterMin { get; set; }

        public DateOnly? ClosingDateFilterMax { get; set; }
        public DateOnly? ExpiringDateFilterMin { get; set; }

        public DateOnly? ExpiringDateFilterMax { get; set; }
        [SelectItems(nameof(ShowDiversityBoolFilterItems))]
        public string ShowDiversityFilter { get; set; }

        public List<SelectListItem> ShowDiversityBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };

        [DisplayName("Company")]
        public Guid CompanyIdFilter { get; set; }

        [SelectItems(nameof(IdentityUserLookupList))]
        public Guid IdentityUserIdFilter { get; set; }
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        [SelectItems(nameof(PracticeGroupLookupList))]
        public Guid PracticeGroupIdFilter { get; set; }
        public List<SelectListItem> PracticeGroupLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        [SelectItems(nameof(FormDefinitionLookupList))]
        public Guid? VacancyFormDefinitionIdFilter { get; set; }
        public List<SelectListItem> FormDefinitionLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        [SelectItems(nameof(FormDefinitionLookupList))]
        public Guid? DiversityFormDefinitionIdFilter { get; set; }

        public List<SelectListItem> VacancyStatusLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("Active", VacancyStatus.Active.ToString()),
            new SelectListItem("Pending", VacancyStatus.Pending.ToString()),
            new SelectListItem("Closed", VacancyStatus.Closed.ToString()),
            new SelectListItem("Expired", VacancyStatus.Expired.ToString())
        };

        [SelectItems(nameof(VacancyStatusLookupList))]
        public VacancyStatus? StatusFilter { get; set; }

        protected IVacanciesAppService _vacanciesAppService;

        public IndexModelBase(IVacanciesAppService vacanciesAppService)
        {
            _vacanciesAppService = vacanciesAppService;
        }

        public virtual async Task OnGetAsync()
        {
            IdentityUserLookupList.AddRange((
                            await _vacanciesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                            {
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                    );

            PracticeGroupLookupList.AddRange((
                            await _vacanciesAppService.GetPracticeGroupLookupAsync(new LookupRequestDto
                            {
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                    );

            FormDefinitionLookupList.AddRange((
                            await _vacanciesAppService.GetFormDefinitionLookupAsync(new LookupRequestDto
                            {
                                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                            })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                    );

            await Task.CompletedTask;
        }
    }
}