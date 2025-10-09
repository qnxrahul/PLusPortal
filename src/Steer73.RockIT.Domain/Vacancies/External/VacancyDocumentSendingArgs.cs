using System;
using Volo.Abp.BackgroundJobs;

namespace Steer73.RockIT.JobApplications.External
{
    [BackgroundJobName("VacancyDocumentSending")]
    public class VacancyDocumentSendingArgs
    {
        public Guid VacancyId { get; set; }
        public bool ShouldUpateBrochure { get; set; }
        public bool ShouldUpateAddtionalFile { get; set; }
    }
}
