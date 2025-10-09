using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Steer73.RockIT.MediaSources;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.RoleTypes;
using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Steer73.RockIT.Vacancies
{

    [AllowAnonymous]
    public abstract class VacanciesAppServiceBase : RockITAppService
    {
        protected IDistributedCache<VacancyDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IVacancyRepository _vacancyRepository;
        protected VacancyManager _vacancyManager;
        protected IRepository<AppFileDescriptors.AppFileDescriptor, Guid> _appFileDescriptorRepository;
        protected IBlobContainer<VacancyFileContainer> _blobContainer;

        protected IRepository<Steer73.RockIT.Companies.Company, Guid> _companyRepository;
        protected IRepository<Volo.Abp.Identity.IdentityUser, Guid> _identityUserRepository;
        protected IRepository<Steer73.RockIT.PracticeGroups.PracticeGroup, Guid> _practiceGroupRepository;
        protected IRepository<Steer73.RockIT.FormDefinitions.FormDefinition, Guid> _formDefinitionRepository;
        protected IUnitOfWorkManager _unitOfWorkManager;
        protected IVacancyMediaSourceRepository _vacancyMediaSourceRepository;
        protected IVacancyRoleTypeRepository _vacancyRoleTypeRepository;
        protected readonly IReadOnlyRepository<MediaSource, Guid> _mediaSourceRepository;
        protected readonly IReadOnlyRepository<RoleType, Guid> _roleTypesRepository;
        protected readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IConfiguration _configuration;

        public VacanciesAppServiceBase(
            IVacancyRepository vacancyRepository,
            VacancyManager vacancyManager, 
            IDistributedCache<VacancyDownloadTokenCacheItem, string> downloadTokenCache, 
            IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository,
            IBlobContainer<VacancyFileContainer> blobContainer, 
            IRepository<Steer73.RockIT.Companies.Company, Guid> companyRepository,
            IRepository<Volo.Abp.Identity.IdentityUser, Guid> identityUserRepository,
            IRepository<Steer73.RockIT.PracticeGroups.PracticeGroup, Guid> practiceGroupRepository,
            IRepository<Steer73.RockIT.FormDefinitions.FormDefinition, Guid> formDefinitionRepository, 
            IUnitOfWorkManager unitOfWorkManager,
            IVacancyMediaSourceRepository vacancyMediaSourceRepository,
            IVacancyRoleTypeRepository vacancyRoleTypeRepository,
            IReadOnlyRepository<MediaSource, Guid> mediaSourceRepository,
            IReadOnlyRepository<RoleType, Guid> roleTypesRepository,
            IBackgroundJobManager backgroundJobManager,
            IConfiguration configuration)
        {
            _downloadTokenCache = downloadTokenCache;
            _vacancyRepository = vacancyRepository;
            _vacancyManager = vacancyManager;
            _companyRepository = companyRepository;
            _identityUserRepository = identityUserRepository;
            _practiceGroupRepository = practiceGroupRepository;
            _formDefinitionRepository = formDefinitionRepository;
            _appFileDescriptorRepository = appFileDescriptorRepository;
            _blobContainer = blobContainer;
            _unitOfWorkManager = unitOfWorkManager;
            _vacancyMediaSourceRepository = vacancyMediaSourceRepository;
            _vacancyRoleTypeRepository = vacancyRoleTypeRepository;
            _mediaSourceRepository = mediaSourceRepository;
            _roleTypesRepository = roleTypesRepository;
            _backgroundJobManager = backgroundJobManager;
            _configuration = configuration;
        }

        public virtual async Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetListAsync(GetVacanciesInput input)
        {
            var totalCount = await _vacancyRepository.GetCountAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity,input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId, input.Status, input.ShowContributionVacancies);
            var items = await _vacancyRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity,input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId, input.Status, input.ShowContributionVacancies, input.Sorting, input.MaxResultCount, input.SkipCount);


            var dtos = ObjectMapper.Map<List<VacancyWithNavigationProperties>, List<VacancyWithNavigationPropertiesDto>>(items);

            var baseUrl = _configuration["App1:PortalBaseUrl"]?.TrimEnd('/');

            foreach (var dto in dtos)
            {
                dto.Vacancy.VacancyDetailUrl = $"{baseUrl}/VacancyDetail/{dto.Vacancy.Id}";
            }
            return new PagedResultDto<VacancyWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = dtos
            };
        }

		[AllowAnonymous]
		public virtual async Task<VacancyWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            var response = await _vacancyRepository.GetWithNavigationPropertiesAsync(id);

            return ObjectMapper.Map<VacancyWithNavigationProperties, VacancyWithNavigationPropertiesDto>
                (response);
        }

        public virtual async Task<VacancyDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Vacancy, VacancyDto>(await _vacancyRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCompanyLookupAsync(LookupRequestDto input)
        {
            var query = (await _companyRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter))
                .OrderBy(x => x.Name);

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Steer73.RockIT.Companies.Company>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Steer73.RockIT.Companies.Company>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            var query = (await _identityUserRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname);

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Volo.Abp.Identity.IdentityUser>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Volo.Abp.Identity.IdentityUser>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetPracticeGroupLookupAsync(LookupRequestDto input)
        {
            var query = (await _practiceGroupRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter))
                .WhereIf(input.IsActive.HasValue && input.IsActive.Value, x => x.IsActive)
                .OrderBy(x => x.Name);

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Steer73.RockIT.PracticeGroups.PracticeGroup>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Steer73.RockIT.PracticeGroups.PracticeGroup>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        public virtual async Task<PagedResultDto<FormDefinitionLookup<Guid>>> GetFormDefinitionLookupAsync(LookupRequestDto input)
        {
            var query = (await _formDefinitionRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Name != null &&
                         x.Name.Contains(input.Filter))
                .OrderBy(x => x.Name);

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Steer73.RockIT.FormDefinitions.FormDefinition>();
            var totalCount = query.Count();
            return new PagedResultDto<FormDefinitionLookup<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Steer73.RockIT.FormDefinitions.FormDefinition>, List<FormDefinitionLookup<Guid>>>(lookupData)
            };
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _vacancyRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<VacancyDto> CreateAsync(VacancyCreateDto input)
        {
            Vacancy? vacancy = null;

            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                vacancy = await _vacancyManager.CreateAsync(
                    (Guid)input.CompanyId!,
                    (Guid)input.IdentityUserId!,
                    input.PracticeGroupIds, 
                    input.VacancyFormDefinitionId,
                    input.DiversityFormDefinitionId, 
                    input.Title, 
                    input.Reference,
                    input.Regions!,
                    input.ContributorIds!,
                    input.Description, 
                    input.ExternalPostingDate, 
                    input.ClosingDate,
                    input.ExpiringDate,
                    input.ShowDiversity,
                    input.Flag_HideVacancy,
                    input.Role, 
                    input.Benefits,
                    input.Location, 
                    input.Salary,
                    input.FormalInterviewDate,
                    input.SecondInterviewDate,
                    input.BrochureFileId, 
                    input.AdditionalFileId,
                    input.LinkedInUrl,
                    input.ExternalRefId);

                await uow.CompleteAsync();
            }

            return ObjectMapper.Map<Vacancy, VacancyDto>(vacancy);
        }

        [AllowAnonymous]
        public virtual async Task<VacancyDto> UpdateAsync(Guid id, VacancyUpdateDto input)
        {
            Vacancy? vacancy = null;

            using (var uow = _unitOfWorkManager.Begin(
                requiresNew: true, isTransactional: false
            ))
            {
                vacancy = await _vacancyManager.UpdateAsync(
                    id,
                    (Guid)input.CompanyId!, 
                    (Guid)input.IdentityUserId!, 
                    input.PracticeGroupIds, 
                    input.VacancyFormDefinitionId, 
                    input.DiversityFormDefinitionId, 
                    input.Title, 
                    input.Reference,
                    input.Regions!, 
                    input.ContributorIds,
                    input.Description, 
                    input.ExternalPostingDate, 
                    input.ClosingDate, 
                    input.ExpiringDate, 
                    input.ShowDiversity,
                    input.Flag_HideVacancy, 
                    input.Role, 
                    input.Benefits, 
                    input.Location, 
                    input.Salary, 
                    input.FormalInterviewDate,
                    input.SecondInterviewDate, 
                    input.BrochureFileId, 
                    input.AdditionalFileId,
                    input.LinkedInUrl,
                    input.ConcurrencyStamp);
                await uow.CompleteAsync();
            }

            return ObjectMapper.Map<Vacancy, VacancyDto>(vacancy);
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
                new VacancyDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Steer73.RockIT.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }

        public async Task<List<MediaSourceDto>> GetMediaSources(IReadOnlyCollection<Guid> ids)
        {
            var mediaSources = await _mediaSourceRepository.GetListAsync(x => ids.Contains(x.Id));

            return ObjectMapper.Map<List<MediaSource>, List<MediaSourceDto>>(mediaSources);
        }

        public async Task<List<RoleTypeDto>> GetRoleTypes(IReadOnlyCollection<Guid> ids)
        {
            var mediaSources = await _roleTypesRepository.GetListAsync(x => ids.Contains(x.Id));

            return ObjectMapper.Map<List<RoleType>, List<RoleTypeDto>>(mediaSources);
        }

        public async Task<List<PracticeGroupDto>> GetPracticeGroups(IReadOnlyCollection<Guid> ids)
        {
            var practiceGroups = await _practiceGroupRepository.GetListAsync(x => ids.Contains(x.Id));

            return ObjectMapper.Map<List<PracticeGroup>, List<PracticeGroupDto>>(practiceGroups);
        }

        public virtual async Task<ListResultDto<VacancyUserAutoCompleteDto>> GetListForUserAutoCompleteAsync(string filter = "")
        {
            var users = (await _identityUserRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(filter),
                    x => x.Name != null &&
                         ((x.Name ?? "") + " " + (x.Surname ?? "")).Contains(filter))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname);

            var dtos = users.Select(u => new VacancyUserAutoCompleteDto
            {
                Id = u.Id,
                Name = $"{u.Name} {u.Surname}"
            }).ToList();

            return new ListResultDto<VacancyUserAutoCompleteDto>(dtos);
        }
    }
}