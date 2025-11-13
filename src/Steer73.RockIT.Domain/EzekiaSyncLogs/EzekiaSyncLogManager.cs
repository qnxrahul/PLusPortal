using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Steer73.RockIT.EzekiaSyncLogs
{
    public class EzekiaSyncLogEntry
    {
        public string EntityType { get; set; } = null!;
        public Guid? EntityId { get; set; }
        public string? ExternalSystemId { get; set; }
        public string Operation { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? CorrelationId { get; set; }
        public int? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? RequestPayload { get; set; }
        public string? ResponsePayload { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorStackTrace { get; set; }
        public string? AdditionalMetadata { get; set; }
        public int RetryCount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsSuccess =>
            string.Equals(Status, "Success", StringComparison.OrdinalIgnoreCase);
    }

    public class EzekiaSyncLogManager : DomainService
    {
        private readonly IEzekiaSyncLogRepository _repository;
        private readonly ILogger _logger;
        private readonly ICurrentTenant _currentTenant;

        public EzekiaSyncLogManager(
            IEzekiaSyncLogRepository repository,
            ILoggerFactory loggerFactory,
            ICurrentTenant currentTenant)
        {
            _repository = repository;
            _currentTenant = currentTenant;
            _logger = loggerFactory.CreateLogger("EzekiaSync");
        }

        public virtual async Task<Guid> LogAsync(EzekiaSyncLogEntry entry, CancellationToken cancellationToken = default)
        {
            Check.NotNull(entry, nameof(entry));

            var log = new EzekiaSyncLog(
                GuidGenerator.Create(),
                _currentTenant.Id,
                entry.Timestamp,
                entry.EntityType,
                entry.EntityId,
                entry.ExternalSystemId,
                entry.Operation,
                entry.Status,
                entry.CorrelationId,
                entry.OwnerId,
                entry.OwnerName,
                entry.OwnerEmail,
                entry.RequestPayload,
                entry.ResponsePayload,
                entry.ErrorMessage,
                entry.ErrorStackTrace,
                entry.AdditionalMetadata,
                entry.RetryCount);

            await _repository.InsertAsync(log, autoSave: true, cancellationToken);

            var logState = new
            {
                entry.EntityType,
                entry.EntityId,
                entry.Operation,
                entry.Status,
                entry.ExternalSystemId,
                entry.CorrelationId,
                entry.OwnerId
            };

            if (entry.IsSuccess)
            {
                _logger.LogInformation("Ezekia sync succeeded {@State}", logState);
            }
            else
            {
                _logger.LogWarning("Ezekia sync failed {@State} Error:{Error}", logState, entry.ErrorMessage);
            }

            return log.Id;
        }
    }
}
