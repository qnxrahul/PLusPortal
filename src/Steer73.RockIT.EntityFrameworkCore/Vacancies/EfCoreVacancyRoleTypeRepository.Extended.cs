using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.RoleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Vacancies
{
    public class EfCoreVacancyRoleTypeRepository : EfCoreVacancyRoleTypeRepositoryBase, IVacancyRoleTypeRepository
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EfCoreVacancyRoleTypeRepository(IDbContextProvider<RockITDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager)
            : base(dbContextProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<RoleType>> GetListOfRoleTypesAsync(string filter = "", CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync())
                .Set<RoleType>()
                .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
                .OrderBy(x => x.Name);

            return await dbSet.ToListAsync(cancellationToken);
        }

        public async Task<List<VacancyRoleType>> GetListOfVacancyRoleTypesAsync(Guid vacancyId, CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync()).Set<VacancyRoleType>();

            return await dbSet.Where(x => x.VacancyId == vacancyId).ToListAsync(cancellationToken);
        }

        public async Task AddOrUpdateVacancyRoleTypes(Guid vacancyId, ICollection<Guid> vacancyRoleTypes, CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync()).Set<VacancyRoleType>();

            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                var rep = ((IRepository<VacancyRoleType>)this);
                await rep.HardDeleteAsync(x => x.VacancyId == vacancyId, autoSave: true, cancellationToken);

                //await DeleteDirectAsync(x => x.VacancyId == vacancyId);

                await uow.CompleteAsync(cancellationToken);
            }

            if (vacancyRoleTypes == null || vacancyRoleTypes.Count == 0)
            {
                return;
            }

            List<VacancyRoleType> vacancyRoleTypeObjs = new();
            foreach (var vacancyRoleTypeId in vacancyRoleTypes.Distinct())
            {
                vacancyRoleTypeObjs.Add(new VacancyRoleType(Guid.NewGuid(), vacancyId, vacancyRoleTypeId));
            }

            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                await InsertManyAsync(vacancyRoleTypeObjs, autoSave: true, cancellationToken);
                await uow.CompleteAsync(cancellationToken);
            }
        }
    }
}
