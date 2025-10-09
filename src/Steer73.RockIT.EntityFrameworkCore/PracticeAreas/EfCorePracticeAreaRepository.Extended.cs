using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;

namespace Steer73.RockIT.PracticeAreas
{
    public class EfCorePracticeAreaRepository : EfCorePracticeAreaRepositoryBase, IPracticeAreaRepository
    {
        public EfCorePracticeAreaRepository(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}