using Microsoft.Extensions.Logging;
using Steer73.RockIT.Domain.External;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Steer73.RockIT.JobApplications.External
{
    public class VacancyDocumentSendingJob : AsyncBackgroundJob<VacancyDocumentSendingArgs>, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IExternalCompanyService _externalCompanyService;

        public VacancyDocumentSendingJob(
            IExternalCompanyService externalCompanyService,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _externalCompanyService = externalCompanyService;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override async Task ExecuteAsync(VacancyDocumentSendingArgs args)
        {
            var vacancyId = args.VacancyId;
            Logger.LogInformation("Starting: Upload of vacancy:{vacancyId} documents into  Ezekia service...", vacancyId);

            _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

            await _externalCompanyService.SendVacancyDocuments(
                args.VacancyId,
                args.ShouldUpateAddtionalFile,
                args.ShouldUpateBrochure,
                cancellationToken: _cancellationTokenProvider.Token);

            Logger.LogInformation("Completed: Upload of vacancy:{vacancyId} documents into Ezekia service...", vacancyId);
        }
    }
}
