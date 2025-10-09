using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Steer73.RockIT.Shared;
using System.Threading;

namespace Steer73.RockIT.JobApplications
{
    public partial interface IJobApplicationsAppService : IApplicationService
    {
        Task<IRemoteStreamContent> GetFileAsync(GetFileInput input);

        Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input);

        Task<PagedResultDto<JobApplicationWithNavigationPropertiesDto>> GetListAsync(GetJobApplicationsInput input);

        Task<JobApplicationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<JobApplicationDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetVacancyLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<JobApplicationDto> CreateAsync(JobApplicationCreateDto input);

        Task<JobApplicationDto> UpdateAsync(Guid id, JobApplicationUpdateDto input);
        Task<Steer73.RockIT.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

        Task<JobApplicationDto> CreateNewJobApplicationCompleteAsync(NewJobApplicationCompleteDto input);

        Task ApproveAsync(Guid id, CancellationToken cancellationToken = default);

        Task ApproveManyAsync(
            List<SelectedJobApplicationDto> jobApplicationDtos,
            CancellationToken cancellationToken = default);

		Task RejectAsync(Guid id, CancellationToken cancellationToken = default);
	}
}