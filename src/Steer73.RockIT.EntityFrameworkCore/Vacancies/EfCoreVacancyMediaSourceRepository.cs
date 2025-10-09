using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.MediaSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.Vacancies
{
    public abstract class EfCoreVacancyMediaSourceRepositoryBase : EfCoreRepository<RockITDbContext, VacancyMediaSource, Guid>
    {
        public EfCoreVacancyMediaSourceRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }


    }
}
