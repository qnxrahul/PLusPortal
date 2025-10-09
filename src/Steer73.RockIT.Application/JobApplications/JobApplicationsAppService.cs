using Steer73.RockIT.Shared;
using Steer73.RockIT.Vacancies;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Steer73.RockIT.Permissions;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.BlobStoring;

namespace Steer73.RockIT.JobApplications
{

    [AllowAnonymous]
    public abstract class JobApplicationsAppServiceBase : RockITAppService
    {
        protected IDistributedCache<JobApplicationDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IJobApplicationRepository _jobApplicationRepository;
        protected JobApplicationManager _jobApplicationManager;
        protected IRepository<AppFileDescriptors.AppFileDescriptor, Guid> _appFileDescriptorRepository;
        protected IBlobContainer<JobApplicationFileContainer> _blobContainer;
        protected IBlobContainer<JobApplicantContainer> _jobApplicantContainer;

        protected IRepository<Steer73.RockIT.Vacancies.Vacancy, Guid> _vacancyRepository;

        public JobApplicationsAppServiceBase(IJobApplicationRepository jobApplicationRepository, JobApplicationManager jobApplicationManager, IDistributedCache<JobApplicationDownloadTokenCacheItem, string> downloadTokenCache, IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository, IBlobContainer<JobApplicationFileContainer> blobContainer, IRepository<Steer73.RockIT.Vacancies.Vacancy, Guid> vacancyRepository
            , IBlobContainer<JobApplicantContainer> jobApplicantContainer
            )
        {
            _downloadTokenCache = downloadTokenCache;
            _jobApplicationRepository = jobApplicationRepository;
            _jobApplicationManager = jobApplicationManager; _vacancyRepository = vacancyRepository;
            _appFileDescriptorRepository = appFileDescriptorRepository;
            _blobContainer = blobContainer;
            _jobApplicantContainer = jobApplicantContainer;
        }

        public virtual async Task<PagedResultDto<JobApplicationWithNavigationPropertiesDto>> GetListAsync(GetJobApplicationsInput input)
        {
            var totalCount = await _jobApplicationRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.EmailAddress, input.Title, input.PhoneNumber, input.Landline, input.CurrentRole, input.CurrentCompany, input.CurrentPositionType, input.VacancyId);
            var items = await _jobApplicationRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.EmailAddress, input.Title, input.PhoneNumber, input.Landline, input.CurrentRole, input.CurrentCompany, input.CurrentPositionType, input.VacancyId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<JobApplicationWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<JobApplicationWithNavigationProperties>, List<JobApplicationWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<JobApplicationWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<JobApplicationWithNavigationProperties, JobApplicationWithNavigationPropertiesDto>
                (await _jobApplicationRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<JobApplicationDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<JobApplication, JobApplicationDto>(await _jobApplicationRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetVacancyLookupAsync(LookupRequestDto input)
        {
            var query = (await _vacancyRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Title != null &&
                         x.Title.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Steer73.RockIT.Vacancies.Vacancy>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Steer73.RockIT.Vacancies.Vacancy>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _jobApplicationRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<JobApplicationDto> CreateAsync(JobApplicationCreateDto input)
        {
            if (input.VacancyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Vacancy"]]);
            }

            var jobApplication = await _jobApplicationManager.CreateAsync(
            input.VacancyId, input.FirstName, input.LastName, input.Aka, input.EmailAddress, JobApplicationConsts.DefaultStatus, JobApplicationConsts.DefaultSyncStatus, JobApplicationConsts.DefaultSyncStatus, JobApplicationConsts.DefaultSyncStatus, input.Title, input.PhoneNumber, input.Landline, input.CurrentRole, input.CurrentCompany, input.CurrentPositionType, input.CVUrl, input.CoverLetterUrl, input.AdditionalDocumentUrl
            );

            return ObjectMapper.Map<JobApplication, JobApplicationDto>(jobApplication);
        }

        [AllowAnonymous]
        public virtual async Task<JobApplicationDto> UpdateAsync(Guid id, JobApplicationUpdateDto input)
        {
            if (input.VacancyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Vacancy"]]);
            }

            var jobApplication = await _jobApplicationManager.UpdateAsync(
            id,
            input.VacancyId, input.FirstName, input.LastName, input.Aka, input.EmailAddress, input.Title, input.PhoneNumber, input.Landline, input.CurrentRole, input.CurrentCompany, input.CurrentPositionType, input.CVUrl, input.CoverLetterUrl, input.AdditionalDocumentUrl, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<JobApplication, JobApplicationDto>(jobApplication);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetFileAsync(GetFileInput input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var fileDescriptor = await _appFileDescriptorRepository.GetAsync(input.FileId);
            var stream = await _blobContainer.GetAsync(fileDescriptor.Id.ToString("N"));

            return new RemoteStreamContent(stream, fileDescriptor.Name, fileDescriptor.MimeType);
        }

        public virtual async Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input)
        {
            var id = GuidGenerator.Create();
            var fileDescriptor = await _appFileDescriptorRepository.InsertAsync(new AppFileDescriptors.AppFileDescriptor(id, input.FileName, input.ContentType));

            await _blobContainer.SaveAsync(fileDescriptor.Id.ToString("N"), input.GetStream());

            return ObjectMapper.Map<AppFileDescriptors.AppFileDescriptor, AppFileDescriptorDto>(fileDescriptor);
        }

        public virtual async Task<Steer73.RockIT.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new JobApplicationDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Steer73.RockIT.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}