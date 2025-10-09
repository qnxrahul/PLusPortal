using System;
using Volo.Abp.BackgroundJobs;

namespace Steer73.RockIT.JobApplications.External
{
    [BackgroundJobName("ApplicantDataSending")]
    public class JobApplicationSendingArgs
    {
        public Guid JobapplicationId { get; set; }
    }
}
