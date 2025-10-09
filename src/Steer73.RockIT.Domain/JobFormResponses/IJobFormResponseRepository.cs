using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.JobFormResponses
{
    public partial interface IJobFormResponseRepository : IRepository<JobFormResponse, Guid>
    {
        Task<List<JobFormResponse>> GetListByJobApplicationIdAsync(
    Guid jobApplicationId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken = default);

        Task<List<JobFormResponse>> GetListAsync(
                    string? filterText = null,

                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,

            CancellationToken cancellationToken = default);
    }
}