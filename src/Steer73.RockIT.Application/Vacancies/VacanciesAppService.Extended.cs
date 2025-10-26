using EzekiaCRM;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.JobApplications.External;
using Steer73.RockIT.MediaSources;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.RoleTypes;
using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Steer73.RockIT.Vacancies
{
    public class VacanciesAppService : VacanciesAppServiceBase, IVacanciesAppService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IClient _ezekiaCRM;
        protected readonly ILogger<VacanciesAppService> _logger;
        private readonly IConfiguration _configuration;

        public VacanciesAppService(
            IVacancyRepository vacancyRepository,
            VacancyManager vacancyManager,
            IDistributedCache<VacancyDownloadTokenCacheItem, string> downloadTokenCache,
            IRepository<AppFileDescriptors.AppFileDescriptor, Guid> appFileDescriptorRepository,
            IBlobContainer<VacancyFileContainer> blobContainer,
            IRepository<Steer73.RockIT.Companies.Company, Guid> companyRepository,
            IRepository<Volo.Abp.Identity.IdentityUser, Guid> identityUserRepository,
            IRepository<Steer73.RockIT.PracticeGroups.PracticeGroup, Guid> practiceGroupRepository,
            IRepository<Steer73.RockIT.FormDefinitions.FormDefinition, Guid> formDefinitionRepository,
            ICurrentUser currentUser,
            IUnitOfWorkManager unitOfWorkManager,
            IVacancyMediaSourceRepository vacancyMediaSourceRepository,
            IVacancyRoleTypeRepository vacancyRoleTypeRepository,
            IReadOnlyRepository<MediaSource, Guid> mediaSourceRepository,
            IReadOnlyRepository<RoleType, Guid> roleTypesRepository,
            IBackgroundJobManager backgroundJobManager,
            IClient ezekiaCRM,
            ILogger<VacanciesAppService> logger,
            IConfiguration configuration)
           : base(
                 vacancyRepository,
                 vacancyManager,
                 downloadTokenCache,
                 appFileDescriptorRepository,
                 blobContainer,
                 companyRepository,
                 identityUserRepository,
                 practiceGroupRepository,
                 formDefinitionRepository,
                 unitOfWorkManager,
                 vacancyMediaSourceRepository,
                 vacancyRoleTypeRepository,
                 mediaSourceRepository,
                 roleTypesRepository,
                 backgroundJobManager,
                 configuration)
        {
            _currentUser = currentUser;
            _ezekiaCRM = ezekiaCRM;
            _logger = logger;
            _configuration = configuration;
        }

        public virtual async Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetListForCurrentUserAsync(GetVacanciesInput input)
        {
            input.IdentityUserId = _currentUser.Id;
            input.ShowContributionVacancies = true;

            var totalCount = await _vacancyRepository.GetCountAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity,input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId, input.Status, input.ShowContributionVacancies);
            var items = await _vacancyRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity,input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId, input.Status, input.ShowContributionVacancies, input.Sorting, input.MaxResultCount, input.SkipCount);

            var baseUrl = _configuration["App1:PortalBaseUrl"]?.TrimEnd('/');

            var dtos = ObjectMapper.Map<List<VacancyWithNavigationProperties>, List<VacancyWithNavigationPropertiesDto>>(items);

            // Add VacancyDetailUrl manually
            foreach (var dto in dtos)
            {
                dto.Vacancy.VacancyDetailUrl = $"{baseUrl}/VacancyDetail/{dto.Vacancy.Id}";
            }

            return new PagedResultDto<VacancyWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<VacancyWithNavigationProperties>, List<VacancyWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<string> GetNewVacancyReferenceNumberAsync(CancellationToken cancellationToken = default)
        {
            var query = (await _vacancyRepository.GetQueryableAsync())
                .Select(x => x.Reference);
            var existingReferences = await _vacancyRepository.AsyncExecuter.ToListAsync(query, cancellationToken);

            string result = null;
            bool exists = true;

            while (exists)
            {
                result = Guid.NewGuid().ToString()[..8];

                exists = existingReferences.Any(x => string.Equals(x, result, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public virtual async Task<PagedResultDto<MediaSourceDto>> GetListOfMediaSourcesAsync(string filter = "", CancellationToken cancellationToken = default)
        {
            var mediaSources = await _vacancyMediaSourceRepository.GetListOfMediaSourcesAsync(filter, cancellationToken: cancellationToken);

            return new PagedResultDto<MediaSourceDto>
            {
                TotalCount = mediaSources.Count,
                Items = ObjectMapper.Map<List<MediaSource>, List<MediaSourceDto>>(mediaSources)
            };
        }

        public virtual async Task<PagedResultDto<RoleTypeDto>> GetListOfRoleTypesAsync(string filter = "", CancellationToken cancellationToken = default)
        {
            var roleTypes = await _vacancyRoleTypeRepository.GetListOfRoleTypesAsync(filter, cancellationToken);

            return new PagedResultDto<RoleTypeDto>
            {
                TotalCount = roleTypes.Count,
                Items = ObjectMapper.Map<List<RoleType>, List<RoleTypeDto>>(roleTypes)
            };
        }

        public virtual async Task<List<VacancyMediaSourceDto>> GetListOfVacancyMediaSourcesAsync(Guid vacancyId, CancellationToken cancellationToken = default)
        {
            return ObjectMapper.Map<List<VacancyMediaSource>, List<VacancyMediaSourceDto>>(await _vacancyMediaSourceRepository.GetListOfVacancyMediaSourcesAsync(vacancyId, cancellationToken));
        }

        public virtual async Task<List<VacancyRoleTypeDto>> GetListOfVacancyRoleTypesAsync(Guid vacancyId, CancellationToken cancellationToken = default)
        {
            return ObjectMapper.Map<List<VacancyRoleType>, List<VacancyRoleTypeDto>>(await _vacancyRoleTypeRepository.GetListOfVacancyRoleTypesAsync(vacancyId, cancellationToken));
        }

        //[Authorize(RockITSharedPermissions.Vacancies.Create)]
        public override async Task<VacancyDto> CreateAsync(VacancyCreateDto input)
        {
            var result = await base.CreateAsync(input);
            await _vacancyMediaSourceRepository.AddOrUpdateVacancyMediaSources(result.Id, input.MediaSourceIds!);
            await _vacancyRoleTypeRepository.AddOrUpdateVacancyRoleTypes(result.Id, input.RoleTypeIds);
            
            await _backgroundJobManager.EnqueueAsync(
                new VacancySendingArgs 
                { 
                    VacancyId = result.Id
                }, 
                BackgroundJobPriority.AboveNormal);

            var documentSendingArgs = new VacancyDocumentSendingArgs 
            {
                VacancyId = result.Id,
                ShouldUpateAddtionalFile = input.AdditionalFileId is not null,
                ShouldUpateBrochure = input.BrochureFileId is not null
            };
            
            if (documentSendingArgs.ShouldUpateBrochure
               || documentSendingArgs.ShouldUpateAddtionalFile)
            {
                // Make sure document is sent after data by using a lower priority level
                await _backgroundJobManager.EnqueueAsync(
                    documentSendingArgs,
                    BackgroundJobPriority.Normal);
            }
           
            return result;
        }

       // [Authorize(RockITSharedPermissions.Vacancies.Edit)]
        public override async Task<VacancyDto> UpdateAsync(Guid id, VacancyUpdateDto input)
        {
            var vacancy = await _vacancyRepository.GetAsync(id);

            var documentSendingArgs = new VacancyDocumentSendingArgs
            { 
                VacancyId = vacancy.Id,

                ShouldUpateAddtionalFile = input.AdditionalFileId is not null
                && (vacancy.AdditionalFileId != input.AdditionalFileId || vacancy.ExternalRefId is null),

                ShouldUpateBrochure = input.BrochureFileId is not null
                && (vacancy.BrochureFileId != input.BrochureFileId || vacancy.ExternalRefId is null) 
            };

            var result = await base.UpdateAsync(id, input);
            await _vacancyMediaSourceRepository.AddOrUpdateVacancyMediaSources(id, input.MediaSourceIds!);
            await _vacancyRoleTypeRepository.AddOrUpdateVacancyRoleTypes(id, input.RoleTypeIds);
         
            return result;
        }

        //[Authorize(RockITSharedPermissions.Vacancies.Create)]
        public virtual async Task<ProjectLookUpDto> GetProjectByIdAsync(string projectId, CancellationToken cancellationToken = default)
        {
            try
            {
                var projects = await _ezekiaCRM.V3ProjectsGetAsync(
                    query: projectId,
                    filterOn: [EzekiaCRM.Anonymous7.ProjectId],
                    sortBy: null,
                    sortOrder: null,
                    fields: null,
                    since: null,
                    before: null,
                    between: null,
                    view: null,
                    withArchived: null,
                    archived: null,
                    fuzzy: null,
                    tags: null,
                    isAssignment: null,
                    isOpportunity: null,
                    isList: null,
                    cancellationToken: cancellationToken
                );

                var project = projects?.Data?.FirstOrDefault();

                if (project is null)
                {
                    throw new UserFriendlyException($"Project with id: {projectId} does not exist on Ezekia");
                }

                var company = await _companyRepository.GetListAsync((comp) => comp.ExternalRefId == project.Relationships.Company.Id);
                
                return new ProjectLookUpDto { 
                    CompanyId = company.FirstOrDefault()?.Id,
                    EzekiaProject = ObjectMapper.Map<default8, EzekiaProjectDto>(project)
                };

			}
            catch (ApiException ex)
            {
                _logger.LogException(ex, LogLevel.Error);
                throw new UserFriendlyException($"Something went wrong");

			}
		}
	}
}
