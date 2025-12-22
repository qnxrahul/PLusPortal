using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.Vacancies;
using Microsoft.Extensions.Configuration; // add this
using Steer73.RockIT.JobApplications;

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
        protected IJobApplicationsAppService _jobApplicationsAppService;

        private readonly IConfiguration _configuration; //  inject configuration

        public string VacancyDetailUrl { get; set; } //  expose to Razor
        public VacancyApplicationsBase(
            IVacanciesAppService vacanciesAppService,
            IJobApplicationsAppService jobApplicationsAppService,
            IConfiguration configuration)
        {
            _vacanciesAppService = vacanciesAppService;
            _jobApplicationsAppService = jobApplicationsAppService;
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

        public virtual async Task<IActionResult> OnGetExportCsvAsync(string? filterText)
        {
            var vacancy = await _vacanciesAppService.GetAsync(Id);

            var input = new GetJobApplicationsInput
            {
                VacancyId = Id,
                FilterText = string.IsNullOrWhiteSpace(filterText) ? null : filterText,
                SkipCount = 0,
                MaxResultCount = 10000,
                Sorting = "CreationTime DESC"
            };

            var result = await _jobApplicationsAppService.GetListAsync(input);

            static string CsvEscape(string? value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return "";
                }

                var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
                if (!needsQuotes)
                {
                    return value;
                }

                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            var sb = new StringBuilder();
            sb.AppendLine(
                "FirstName,LastName,EmailAddress,Title,PhoneNumber,Landline,CurrentRole,CurrentCompany,CurrentPositionType,Status,ApplicationDateAndTime,CVDownloadUrl,CoverLetterDownloadUrl,AdditionalDocumentDownloadUrl");

            foreach (var item in result.Items)
            {
                var ja = item.JobApplication;
                var baseApi = $"{Request.Scheme}://{Request.Host}";

                string? cvLink = string.IsNullOrWhiteSpace(ja.CVUrl)
                    ? null
                    : $"{baseApi}/api/app/job-applications/file-by-type?fileType=CV&jobApplicationId={ja.Id}";
                string? coverLink = string.IsNullOrWhiteSpace(ja.CoverLetterUrl)
                    ? null
                    : $"{baseApi}/api/app/job-applications/file-by-type?fileType=CoverLetter&jobApplicationId={ja.Id}";
                string? additionalLink = string.IsNullOrWhiteSpace(ja.AdditionalDocumentUrl)
                    ? null
                    : $"{baseApi}/api/app/job-applications/file-by-type?fileType=AdditionalDocument&jobApplicationId={ja.Id}";

                var created = ja.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");

                sb.AppendLine(string.Join(",",
                    CsvEscape(ja.FirstName),
                    CsvEscape(ja.LastName),
                    CsvEscape(ja.EmailAddress),
                    CsvEscape(ja.Title),
                    CsvEscape(ja.PhoneNumber),
                    CsvEscape(ja.Landline),
                    CsvEscape(ja.CurrentRole),
                    CsvEscape(ja.CurrentCompany),
                    CsvEscape(ja.CurrentPositionType),
                    CsvEscape(ja.StatusAsString ?? ja.Status?.ToString()),
                    CsvEscape(created),
                    CsvEscape(cvLink),
                    CsvEscape(coverLink),
                    CsvEscape(additionalLink)
                ));
            }

            var fileBaseName = !string.IsNullOrWhiteSpace(vacancy.ProjectId)
                ? vacancy.ProjectId
                : vacancy.Reference;

            var fileName = $"Applicants_{fileBaseName}_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv";
            var bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(sb.ToString());
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }
    }
}