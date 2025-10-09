using Microsoft.Extensions.Logging;
using Steer73.RockIT.Domain.External;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Steer73.RockIT.JobApplications.External
{
    public class ApplicantDocumentSendingJob : AsyncBackgroundJob<JobApplicationDocumentSendingArgs>, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IExternalCompanyService _externalCompanyService;

        public ApplicantDocumentSendingJob(
            IExternalCompanyService externalCompanyService,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _externalCompanyService = externalCompanyService;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override async Task ExecuteAsync(JobApplicationDocumentSendingArgs args)
        {
            var applicationId = args.JobApplicationId;
            Logger.LogInformation("Starting: Upload of application:{applicantionId} documents into  Ezekia service...", applicationId);

            _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

            await _externalCompanyService.SendApplicantDocuments(
                jobApplicationId: args.JobApplicationId,
                cancellationToken: _cancellationTokenProvider.Token);

            Logger.LogInformation("Completed: Upload of application:{applicantionId} documents into Ezekia service...", applicationId);
        }
    }
}
