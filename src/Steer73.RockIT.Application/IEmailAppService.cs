using Steer73.RockIT.BrochureSubscriptions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System.Threading.Tasks;

namespace Steer73.RockIT
{
    public interface IEmailAppService
    {
        Task QueueBrochureEmail(
            string emailTemplatePath,
            string brochureLink,
            BrochureSubscriptionDto subscription,
            VacancyWithNavigationPropertiesDto vacancy);
        Task QueueNoBrochureEmail(
            string emailTemplatePath,
            string brochureLink,
            BrochureSubscriptionDto subscription,
            VacancyWithNavigationPropertiesDto vacancy);

        Task QueueNewJobApplicationEmail(
            string emailTemplatePath,
            JobApplicationDto jobApplication,
            VacancyWithNavigationPropertiesDto vacancy);

	}
}
