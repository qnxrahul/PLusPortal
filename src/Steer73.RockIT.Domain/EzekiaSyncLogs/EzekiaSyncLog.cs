using System;
using Steer73.RockIT.EzekiaSyncLogs;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Steer73.RockIT.EzekiaSyncLogs
{
    public class EzekiaSyncLog : Entity<Guid>, IMultiTenant
    {
        protected EzekiaSyncLog()
        {
        }

        public EzekiaSyncLog(
            Guid id,
            Guid? tenantId,
            DateTime timestamp,
            string entityType,
            Guid? entityId,
            string? externalSystemId,
            string operation,
            string status,
            string? correlationId,
            int? ownerId,
            string? ownerName,
            string? ownerEmail,
            string? requestPayload,
            string? responsePayload,
            string? errorMessage,
            string? errorStackTrace,
            string? additionalMetadata,
            int retryCount)
        {
            Id = id;
            TenantId = tenantId;
            Timestamp = timestamp;
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            EntityId = entityId;
            ExternalSystemId = Check.Length(externalSystemId, nameof(externalSystemId), EzekiaSyncLogConsts.ExternalSystemIdMaxLength);
            Operation = Check.NotNullOrWhiteSpace(operation, nameof(operation), EzekiaSyncLogConsts.OperationMaxLength);
            Status = Check.NotNullOrWhiteSpace(status, nameof(status), EzekiaSyncLogConsts.StatusMaxLength);
            CorrelationId = Check.Length(correlationId, nameof(correlationId), EzekiaSyncLogConsts.CorrelationIdMaxLength);
            OwnerId = ownerId;
            OwnerName = Check.Length(ownerName, nameof(ownerName), EzekiaSyncLogConsts.OwnerNameMaxLength);
            OwnerEmail = Check.Length(ownerEmail, nameof(ownerEmail), EzekiaSyncLogConsts.OwnerEmailMaxLength);
            RequestPayload = Check.Length(requestPayload, nameof(requestPayload), EzekiaSyncLogConsts.RequestPayloadMaxLength);
            ResponsePayload = Check.Length(responsePayload, nameof(responsePayload), EzekiaSyncLogConsts.ResponsePayloadMaxLength);
            ErrorMessage = Check.Length(errorMessage, nameof(errorMessage), EzekiaSyncLogConsts.ErrorMessageMaxLength);
            ErrorStackTrace = Check.Length(errorStackTrace, nameof(errorStackTrace), EzekiaSyncLogConsts.ErrorStackTraceMaxLength);
            AdditionalMetadata = Check.Length(additionalMetadata, nameof(additionalMetadata), EzekiaSyncLogConsts.AdditionalMetadataMaxLength);
            RetryCount = retryCount;
        }

        public Guid? TenantId { get; protected set; }
        public DateTime Timestamp { get; protected set; }
        public string EntityType { get; protected set; } = null!;
        public Guid? EntityId { get; protected set; }
        public string? ExternalSystemId { get; protected set; }
        public string Operation { get; protected set; } = null!;
        public string Status { get; protected set; } = null!;
        public string? CorrelationId { get; protected set; }
        public int? OwnerId { get; protected set; }
        public string? OwnerName { get; protected set; }
        public string? OwnerEmail { get; protected set; }
        public string? RequestPayload { get; protected set; }
        public string? ResponsePayload { get; protected set; }
        public string? ErrorMessage { get; protected set; }
        public string? ErrorStackTrace { get; protected set; }
        public string? AdditionalMetadata { get; protected set; }
        public int RetryCount { get; protected set; }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
