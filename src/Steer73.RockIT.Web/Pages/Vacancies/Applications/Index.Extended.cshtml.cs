using Microsoft.Extensions.Configuration;
using Steer73.RockIT.Vacancies;

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public class VacancyApplicationsModel : VacancyApplicationsBase
    {
        public VacancyApplicationsModel(IVacanciesAppService vacanciesAppService,
            IConfiguration configuration)
            : base(vacanciesAppService, configuration)
        {
        }
    }
}