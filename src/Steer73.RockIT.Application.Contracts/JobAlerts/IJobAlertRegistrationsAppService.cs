using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.JobAlerts;

public interface IJobAlertRegistrationsAppService : IApplicationService
{
    Task<JobAlertRegistrationDto> RegisterAsync(JobAlertRegistrationCreateDto input);

    Task<PagedResultDto<JobAlertRegistrationDto>> GetListAsync(JobAlertRegistrationListInput input);

    Task<JobAlertRegistrationDto> GetAsync(Guid id);

    Task UnsubscribeByEmailAsync(JobAlertUnsubscribeDto input);

    Task UnsubscribeByTokenAsync(JobAlertUnsubscribeTokenDto input);
}
