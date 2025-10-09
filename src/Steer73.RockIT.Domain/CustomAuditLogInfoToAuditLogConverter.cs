//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Volo.Abp.AspNetCore.ExceptionHandling;
//using Volo.Abp.Auditing;
//using Volo.Abp.Data;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Guids;
//using Volo.Abp.Http;
//using Volo.Abp.Json;

//namespace Steer73.RockIT;

//[Dependency(ReplaceServices = true)]
//[ExposeServices(typeof(AuditLogInfoToAuditLogConverter), typeof(IAuditLogInfoToAuditLogConverter))]
//public class CustomAuditLogInfoToAuditLogConverter : AuditLogInfoToAuditLogConverter
//{
//    public CustomAuditLogInfoToAuditLogConverter(
//        IGuidGenerator guidGenerator,
//        IExceptionToErrorInfoConverter exceptionToErrorInfoConverter,
//        IJsonSerializer jsonSerializer,
//        IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions) : base(
//            guidGenerator,
//            exceptionToErrorInfoConverter,
//            jsonSerializer,
//            exceptionHandlingOptions)
//    {
//    }

//    public override Task<AuditLog> ConvertAsync(AuditLogInfo auditLogInfo)
//    {
//        Guid auditLogId = GuidGenerator.Create();
//        ExtraPropertyDictionary extraPropertyDictionary = new ExtraPropertyDictionary();
//        if (auditLogInfo.ExtraProperties != null)
//        {
//            foreach (KeyValuePair<string, object> extraProperty in auditLogInfo.ExtraProperties)
//            {
//                extraPropertyDictionary.Add(extraProperty.Key, extraProperty.Value);
//            }
//        }

//        List<EntityChange> entityChanges = auditLogInfo.EntityChanges?.Select((EntityChangeInfo entityChangeInfo) => new EntityChange(GuidGenerator, auditLogId, entityChangeInfo, auditLogInfo.TenantId)).ToList() ?? new List<EntityChange>();
//        List<AuditLogAction> actions = auditLogInfo.Actions?.Select((AuditLogActionInfo auditLogActionInfo) => new AuditLogAction(GuidGenerator.Create(), auditLogId, auditLogActionInfo, auditLogInfo.TenantId)).ToList() ?? new List<AuditLogAction>();
//        IEnumerable<RemoteServiceErrorInfo> enumerable = auditLogInfo.Exceptions?.Select((Exception exception) => ExceptionToErrorInfoConverter.Convert(exception, delegate (AbpExceptionHandlingOptions options)
//        {
//            #region CustomCode
//            // Force saving exception details in audit log
//            options.SendExceptionsDetailsToClients = true;
//            options.SendStackTraceToClients = true;
//            #endregion
//        })) ?? new List<RemoteServiceErrorInfo>();
//        string exceptions = (enumerable.Any() ? JsonSerializer.Serialize(enumerable, camelCase: true, indented: true) : null);
//        string comments = auditLogInfo.Comments?.JoinAsString(Environment.NewLine);
//        return Task.FromResult(new AuditLog(auditLogId, auditLogInfo.ApplicationName, auditLogInfo.TenantId, auditLogInfo.TenantName, auditLogInfo.UserId, auditLogInfo.UserName, auditLogInfo.ExecutionTime, auditLogInfo.ExecutionDuration, auditLogInfo.ClientIpAddress, auditLogInfo.ClientName, auditLogInfo.ClientId, auditLogInfo.CorrelationId, auditLogInfo.BrowserInfo, auditLogInfo.HttpMethod, auditLogInfo.Url, auditLogInfo.HttpStatusCode, auditLogInfo.ImpersonatorUserId, auditLogInfo.ImpersonatorUserName, auditLogInfo.ImpersonatorTenantId, auditLogInfo.ImpersonatorTenantName, extraPropertyDictionary, entityChanges, actions, exceptions, comments));
//    }
//}
