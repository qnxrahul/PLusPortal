using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.JobApplications
{
    public partial interface IJobApplicationRepository : IRepository<JobApplication, Guid>
    {
        Task<JobApplicationWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<JobApplicationWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            Guid? vacancyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<JobApplication>> GetListAsync(
                    string? filterText = null,
                    string? firstName = null,
                    string? lastName = null,
                    string? emailAddress = null,
                    string? title = null,
                    string? phoneNumber = null,
                    string? landline = null,
                    string? currentRole = null,
                    string? currentCompany = null,
                    string? currentPositionType = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            Guid? vacancyId = null,
            CancellationToken cancellationToken = default);
    }
}