using Steer73.RockIT.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.Vacancies
{
    public abstract class EfCoreVacancyRoleTypeRepositoryBase : EfCoreRepository<RockITDbContext, VacancyRoleType, Guid>
    {
        public EfCoreVacancyRoleTypeRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
