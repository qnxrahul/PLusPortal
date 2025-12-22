using Microsoft.Extensions.Configuration;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public class VacancyApplicationsModel : VacancyApplicationsBase
    {
        public VacancyApplicationsModel(
            IVacanciesAppService vacanciesAppService,
            IJobApplicationsAppService jobApplicationsAppService,
            IConfiguration configuration)
            : base(vacanciesAppService, jobApplicationsAppService, configuration)
        {
        }
    }
}