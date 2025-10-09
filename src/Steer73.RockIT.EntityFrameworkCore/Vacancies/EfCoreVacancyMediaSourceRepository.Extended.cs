using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.MediaSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Vacancies
{
    public class EfCoreVacancyMediaSourceRepository : EfCoreVacancyMediaSourceRepositoryBase, IVacancyMediaSourceRepository
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        public EfCoreVacancyMediaSourceRepository(IDbContextProvider<RockITDbContext> dbContextProvider, IUnitOfWorkManager unitOfWorkManager)
            : base(dbContextProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<MediaSource>> GetListOfMediaSourcesAsync(string filter, CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync())
                .Set<MediaSource>()
                .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Name.Contains(filter))
                .OrderBy(x => x.Name);

            return await dbSet.ToListAsync(cancellationToken);
        }

        public async Task<List<VacancyMediaSource>> GetListOfVacancyMediaSourcesAsync(Guid vacancyId, CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync()).Set<VacancyMediaSource>();

            return await dbSet.Where(x => x.VacancyId == vacancyId).ToListAsync(cancellationToken);
        }

        public async Task AddOrUpdateVacancyMediaSources(Guid vacancyId, ICollection<Guid> vacancyMediaSources, CancellationToken cancellationToken = default)
        {
            var dbSet = (await GetDbContextAsync()).Set<VacancyMediaSource>();
           
            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                var rep = ((IRepository<VacancyMediaSource>)this);
                await rep.HardDeleteAsync(x => x.VacancyId == vacancyId, autoSave: true, cancellationToken);
                await uow.CompleteAsync(cancellationToken);
            }

            if (vacancyMediaSources == null || vacancyMediaSources.Count == 0)
            {
                return;
            }

            List<VacancyMediaSource> vacancyMediaSourceObjs = new();
            foreach (var vacancyMediaSourceId in vacancyMediaSources.Distinct())
            {
                vacancyMediaSourceObjs.Add(new VacancyMediaSource(Guid.NewGuid(), vacancyId, vacancyMediaSourceId));
            }

            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                await InsertManyAsync(vacancyMediaSourceObjs, autoSave: true, cancellationToken);
                await uow.CompleteAsync(cancellationToken);
            }

        }
    }
}
