using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.DiversityFormResponses
{
    public partial interface IDiversityFormResponseRepository : IRepository<DiversityFormResponse, Guid>
    {
        Task<List<DiversityFormResponse>> GetListByJobApplicationIdAsync(
    Guid jobApplicationId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken = default);

        Task<List<DiversityFormResponse>> GetListAsync(
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