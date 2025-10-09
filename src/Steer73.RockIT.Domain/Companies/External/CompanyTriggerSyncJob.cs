using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steer73.RockIT.Domain.External;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Steer73.RockIT.Companies.External
{
    public class CompanyTriggerSyncJob : AsyncBackgroundJob<CompanyTriggerSyncArgs>, ITransientDependency
    {
        private readonly IExternalCompanyService _externalCompanyService;
        private readonly EzekiaConfiguration _ezekiaConfiguration;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public CompanyTriggerSyncJob(
            IExternalCompanyService externalCompanyService,
            IOptions<EzekiaConfiguration> ezekiaConfiguration,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _externalCompanyService = externalCompanyService;
            _ezekiaConfiguration = ezekiaConfiguration.Value;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override async Task ExecuteAsync(CompanyTriggerSyncArgs args)
        {
            using var activity = new Activity("ManualCompanySync").Start();

            Logger.LogInformation("Starting: Triggered manually download of list of companies from Ezekia service...");

            //Do the work
            await _externalCompanyService.UpdateCompaniesAsync(
                resultsPerPage: _ezekiaConfiguration.CompaniesPerPage,
                maxIterations: _ezekiaConfiguration.MaxIterations,
                _cancellationTokenProvider.Token);

            Logger.LogInformation("Completed: Triggered manually download of list of companies from Ezekia service...");
        }
    }
}
