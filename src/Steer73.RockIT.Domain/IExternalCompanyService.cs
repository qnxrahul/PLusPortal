using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.Domain.External
{
    public interface IExternalCompanyService : IDomainService
    {
        Task UpdateCompaniesAsync(int resultsPerPage = 50, int maxIterations = 100000, CancellationToken cancellationToken = default);
       
        Task SendApplicantDataAsync(
            Guid jobApplicationId,
            CancellationToken cancellationToken = default);

        Task SendApplicantDocuments(
            Guid jobApplicationId,
            CancellationToken cancellationToken = default);

        Task SendVacancyDataAsync(
            Guid vacancyId,
            CancellationToken cancellationToken = default);

        Task SendVacancyDocuments(
            Guid vacancyId,
            bool shouldUpdateAdditionalFile,
            bool shouldUpdateBrochure,
            CancellationToken cancellationToken = default);
    }
}
