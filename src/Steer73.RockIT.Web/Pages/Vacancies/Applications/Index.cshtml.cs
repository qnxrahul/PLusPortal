using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.Vacancies;
using Microsoft.Extensions.Configuration; // add this

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public abstract class VacancyApplicationsBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public VacancyDto Vacancy { get; set; }

        public List<SelectListItem> SelectedRoleTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SelectedMediaSources { get; set; } = new List<SelectListItem>();

        protected IVacanciesAppService _vacanciesAppService;

        private readonly IConfiguration _configuration; //  inject configuration

        public string VacancyDetailUrl { get; set; } //  expose to Razor
        public VacancyApplicationsBase(IVacanciesAppService vacanciesAppService, IConfiguration configuration)
        {
            _vacanciesAppService = vacanciesAppService;
            _configuration = configuration;
            Vacancy = new();
        }

        public virtual async Task OnGetAsync()
        {
            var vacancyWithNavigationPropertiesDto = await _vacanciesAppService.GetWithNavigationPropertiesAsync(Id);
            Vacancy = vacancyWithNavigationPropertiesDto.Vacancy;

            // build dynamic Vacancy URL here
            var baseUrl = _configuration["App1:PortalBaseUrl"]?.TrimEnd('/');
            VacancyDetailUrl = $"{baseUrl}/VacancyDetail/{Vacancy.Id}";

            var mediaSources = (await _vacanciesAppService.GetListOfMediaSourcesAsync()).Items;
            var selectedMediaSources = await _vacanciesAppService.GetListOfVacancyMediaSourcesAsync(Id);
            SelectedMediaSources.AddRange(selectedMediaSources.Select(x => new SelectListItem(mediaSources.FirstOrDefault(ms => ms.Id == x.MediaSourceId)?.Name, x.MediaSourceId.ToString())));

            var roleTypes = (await _vacanciesAppService.GetListOfRoleTypesAsync()).Items;
            var selectRoleTypes = await _vacanciesAppService.GetListOfVacancyRoleTypesAsync(Id);
            SelectedRoleTypes.AddRange(selectRoleTypes.Select(x => new SelectListItem(roleTypes.FirstOrDefault(rt => rt.Id == x.RoleTypeId)?.Name, x.RoleTypeId.ToString())));
        }
    }
}