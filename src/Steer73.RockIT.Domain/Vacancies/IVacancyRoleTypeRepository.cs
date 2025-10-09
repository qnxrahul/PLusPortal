using Steer73.RockIT.RoleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.Vacancies
{
    public partial interface IVacancyRoleTypeRepository : IRepository<VacancyRoleType, Guid>
    {
        Task<List<RoleType>> GetListOfRoleTypesAsync(string filter = "", CancellationToken cancellationToken = default);
        Task<List<VacancyRoleType>> GetListOfVacancyRoleTypesAsync(Guid vacancyId, CancellationToken cancellationToken = default);
        Task AddOrUpdateVacancyRoleTypes(Guid vacancyId, ICollection<Guid> vacancyRoleTypes, CancellationToken cancellationToken = default);

    }
}
