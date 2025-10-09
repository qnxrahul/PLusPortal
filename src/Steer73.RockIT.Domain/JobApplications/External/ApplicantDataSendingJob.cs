using Microsoft.Extensions.Logging;
using Steer73.RockIT.Domain.External;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Steer73.RockIT.JobApplications.External
{
    public class ApplicantDataSendingJob : AsyncBackgroundJob<JobApplicationSendingArgs>, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IExternalCompanyService _externalCompanyService;

        public ApplicantDataSendingJob(
            IExternalCompanyService externalCompanyService,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _externalCompanyService = externalCompanyService;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override async Task ExecuteAsync(JobApplicationSendingArgs args)
        {
            Logger.LogInformation("Starting: Upload of applicant data into  Ezekia service...");

            _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

            await _externalCompanyService.SendApplicantDataAsync(
                jobApplicationId: args.JobapplicationId,
                cancellationToken: _cancellationTokenProvider.Token
            );

            Logger.LogInformation("Completed: Upload of appicant data into Ezekia service...");
        }
    }
}
