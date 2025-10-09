using System;
using Volo.Abp.BackgroundJobs;

namespace Steer73.RockIT.JobApplications.External
{
    [BackgroundJobName("VacancyDataSending")]
    public class VacancySendingArgs
    {
        public Guid VacancyId { get; set; }
    }
}
