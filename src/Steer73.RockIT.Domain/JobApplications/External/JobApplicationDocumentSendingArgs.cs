using System;
using Volo.Abp.BackgroundJobs;

namespace Steer73.RockIT.JobApplications.External
{
    [BackgroundJobName("ApplicantDocumentSending")]
    public class JobApplicationDocumentSendingArgs
    {
        public Guid JobApplicationId { get; set; }
    }
}
