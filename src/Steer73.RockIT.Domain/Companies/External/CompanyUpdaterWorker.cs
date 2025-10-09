using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steer73.RockIT.Domain.External;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Steer73.RockIT.Companies.External
{
    public class CompanyUpdaterWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public CompanyUpdaterWorker(    
            AbpAsyncTimer timer,    
            IServiceScopeFactory serviceScopeFactory,    
            IOptions<EzekiaConfiguration> _externalConfig) : base(
                timer,
                serviceScopeFactory)
        {
            Timer.Period = _externalConfig.Value.UpdatedPeriodInMs;
        }

        protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            using var activity = new Activity("ScheduledCompanySync").Start();

            Logger.LogInformation("Starting: Download of list of companies from Ezekia service...");

            //Resolve dependencies
            var ezekiaConfig = workerContext.ServiceProvider.GetService<IOptions<EzekiaConfiguration>>()!.Value;
            var externalCompanyService = workerContext
                .ServiceProvider
                .GetRequiredService<IExternalCompanyService>();

            //Do the work
            await externalCompanyService.UpdateCompaniesAsync(
                resultsPerPage: ezekiaConfig.CompaniesPerPage,
                maxIterations: ezekiaConfig.MaxIterations,
                workerContext.CancellationToken);

            Logger.LogInformation("Completed: Download of list of companies from Ezekia service...");
        }
    }
}
