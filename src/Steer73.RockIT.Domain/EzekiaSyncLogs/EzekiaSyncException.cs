using System;
using Volo.Abp;

namespace Steer73.RockIT.EzekiaSyncLogs
{
    public class EzekiaSyncException : BusinessException
    {
        public Guid LogId { get; }
        public DateTime TimestampUtc { get; }

        public EzekiaSyncException(string message, Guid logId, DateTime timestampUtc, Exception? innerException = null)
            : base(code: "EzekiaSync:Error", message: message, innerException: innerException)
        {
            LogId = logId;
            TimestampUtc = timestampUtc;

            WithData(nameof(LogId), logId);
            WithData(nameof(TimestampUtc), timestampUtc);
        }
    }
}
