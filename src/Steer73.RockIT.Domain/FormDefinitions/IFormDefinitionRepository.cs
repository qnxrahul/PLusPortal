using Steer73.RockIT.FormDefinitions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.FormDefinitions
{
    public partial interface IFormDefinitionRepository : IRepository<FormDefinition, Guid>
    {
        Task<List<FormDefinition>> GetListAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            CancellationToken cancellationToken = default);

        Task<List<FormDefinitionWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);
    }
}