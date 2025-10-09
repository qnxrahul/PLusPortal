using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public partial interface IBrochureSubscriptionRepository : IRepository<BrochureSubscription, Guid>
    {
    }
}
