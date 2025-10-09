using Steer73.RockIT.JobApplications;

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public class ViewModalModel : ViewModalModelBase
	{
        public ViewModalModel(IJobApplicationsAppService jobApplicationAppService)
            : base(jobApplicationAppService)
        {
        }
    }
}