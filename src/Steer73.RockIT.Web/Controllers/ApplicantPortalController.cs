using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.Enums;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Validation;

namespace Steer73.RockIT.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApplicantPortalController : AbpController, IValidationEnabled
    {
        private readonly IJobApplicationsAppService _jobApplicationsAppService;
        private readonly IVacanciesAppService _vacanciesAppService;
        private readonly IEmailAppService _emailAppService;

        public ApplicantPortalController(
            IJobApplicationsAppService jobApplicationsAppService,
            IVacanciesAppService vacanciesAppService,
            IEmailAppService emailAppService)
        {
            _jobApplicationsAppService = jobApplicationsAppService;
            _emailAppService = emailAppService;
            _vacanciesAppService = vacanciesAppService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetVacanciesAsync()
        {
            var vacancies = await _jobApplicationsAppService.GetListAsync(new GetJobApplicationsInput());
            return Ok(vacancies);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateJobApplicationAsync([FromForm] JobApplicationModel jobApplication)
        {
            var dto = ObjectMapper.Map<JobApplicationModel, NewJobApplicationCompleteDto>(jobApplication);

            if (jobApplication.FileCv?.Length > 0)
            {           
                dto.FileCv = new BlobDto
                {
                    Content = await jobApplication.FileCv.GetAllBytesAsync(),
                    Name = jobApplication.FileCv.FileName
                };
            }

            if (jobApplication.FileCoverLetter?.Length > 0)
            {
                dto.FileCoverLetter = new BlobDto
                {
                    Content = await jobApplication.FileCoverLetter.GetAllBytesAsync(),
                    Name = jobApplication.FileCoverLetter.FileName
                };
            }

            if (jobApplication.FileAdditionalDoc?.Length > 0)
            {
                dto.FileAdditionalDoc = new BlobDto
                {
                    Content = await jobApplication.FileAdditionalDoc.GetAllBytesAsync(),
                    Name = jobApplication.FileAdditionalDoc.FileName
                };
            }

            var newJobApplication = await _jobApplicationsAppService.CreateNewJobApplicationCompleteAsync(dto);
			var vacancy = await _vacanciesAppService.GetWithNavigationPropertiesAsync(jobApplication.VacancyId);

            var emailTemplate = vacancy.Vacancy.BrochureFileId is not null
                ? RockITWebConstants.NewJobApplicationTemplateEmailPath : RockITWebConstants.NewJobApplicationNoBrochureTemplateEmailPath;

            await _emailAppService.QueueNewJobApplicationEmail(   
                emailTemplate,
                newJobApplication,
                vacancy);

            return Ok(newJobApplication);
        }
    }

    public class JobApplicationModel : NewJobApplicationCompleteBaseDto
    {
        [Display(Name = "CV")]
        [Required]
        public IFormFile FileCv { get; set; } = null!;

        [Display(Name = "Cover Letter")]
        [Required]
        public IFormFile FileCoverLetter { get; set; } = null!;

        [Display(Name = "Additional Doc")]
        public IFormFile? FileAdditionalDoc { get; set; }
    }
}