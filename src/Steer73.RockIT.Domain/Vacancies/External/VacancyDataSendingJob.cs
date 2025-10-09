using Microsoft.Extensions.Logging;
using Steer73.RockIT.Domain.External;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Steer73.RockIT.JobApplications.External
{
    public class VacancyDataSendingJob : AsyncBackgroundJob<VacancySendingArgs>, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IExternalCompanyService _externalCompanyService;

        public VacancyDataSendingJob(
            IExternalCompanyService externalCompanyService,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _externalCompanyService = externalCompanyService;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override async Task ExecuteAsync(VacancySendingArgs args)
        {
            Logger.LogInformation("Starting: Upload of vacancy {0} data into  Ezekia service...", args.VacancyId);

            _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

            await _externalCompanyService.SendVacancyDataAsync(
                vacancyId: args.VacancyId,
                cancellationToken: _cancellationTokenProvider.Token
            );

            Logger.LogInformation("Completed: Upload of vacancy {0} data into Ezekia service...", args.VacancyId);
        }
    }
}
