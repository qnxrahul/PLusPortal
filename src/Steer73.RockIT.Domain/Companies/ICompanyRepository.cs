using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.Companies
{
    public partial interface ICompanyRepository : IRepository<Company, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null,
            CancellationToken cancellationToken = default);
        Task<List<Company>> GetListAsync(
                    string? filterText = null,
                    string? name = null,
                    string? phone = null,
                    string? address = null,
                    string? postcode = null,
                    string? primaryContact = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Company>> GetByExternalRefIds(
            List<int> externalRefIds,
            CancellationToken cancellationToken = default);
    }
}