using Steer73.RockIT.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class EfCoreBrochureSubscriptionRepository : EfCoreRepository<RockITDbContext, BrochureSubscription, Guid>, IBrochureSubscriptionRepository
    {
        public EfCoreBrochureSubscriptionRepository(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
