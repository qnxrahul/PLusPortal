using System;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.EzekiaSyncLogs
{
    public interface IEzekiaSyncLogRepository : IRepository<EzekiaSyncLog, Guid>
    {
    }
}
