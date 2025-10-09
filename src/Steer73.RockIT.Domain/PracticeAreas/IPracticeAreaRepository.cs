using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.PracticeAreas
{
    public partial interface IPracticeAreaRepository : IRepository<PracticeArea, Guid>
    {
        Task<List<PracticeArea>> GetListByPracticeGroupIdAsync(
    Guid practiceGroupId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByPracticeGroupIdAsync(Guid practiceGroupId, CancellationToken cancellationToken = default);

        Task<List<PracticeArea>> GetListAsync(
                    string? filterText = null,
                    string? name = null,
                    bool? isActive = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);
    }
}