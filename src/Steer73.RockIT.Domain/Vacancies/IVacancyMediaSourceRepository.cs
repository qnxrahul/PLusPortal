using Steer73.RockIT.MediaSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.Vacancies
{
    public partial interface IVacancyMediaSourceRepository : IRepository<VacancyMediaSource, Guid>
    {
        Task<List<MediaSource>> GetListOfMediaSourcesAsync(string filter, CancellationToken cancellationToken = default);
        Task<List<VacancyMediaSource>> GetListOfVacancyMediaSourcesAsync(Guid vacancyId, CancellationToken cancellationToken = default);
        Task AddOrUpdateVacancyMediaSources(Guid vacancyId, ICollection<Guid> vacancyMediaSources, CancellationToken cancellationToken = default);
    }
}
