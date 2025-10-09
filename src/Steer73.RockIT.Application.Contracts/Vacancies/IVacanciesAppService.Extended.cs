using Steer73.RockIT.PracticeGroups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Vacancies
{
    public partial interface IVacanciesAppService
    {
        Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetListForCurrentUserAsync(GetVacanciesInput input);
        Task<string> GetNewVacancyReferenceNumberAsync(CancellationToken cancellationToken = default);
        Task<PagedResultDto<MediaSourceDto>> GetListOfMediaSourcesAsync(string filter = "", CancellationToken cancellationToken = default);
        Task<PagedResultDto<RoleTypeDto>> GetListOfRoleTypesAsync(string filter = "", CancellationToken cancellationToken = default);
        Task<List<VacancyMediaSourceDto>> GetListOfVacancyMediaSourcesAsync(Guid vacancyId, CancellationToken cancellationToken = default);
        Task<List<VacancyRoleTypeDto>> GetListOfVacancyRoleTypesAsync(Guid vacancyId, CancellationToken cancellationToken = default);
        Task<List<MediaSourceDto>> GetMediaSources(IReadOnlyCollection<Guid> ids);
        Task<List<RoleTypeDto>> GetRoleTypes(IReadOnlyCollection<Guid> ids);
        Task<List<PracticeGroupDto>> GetPracticeGroups(IReadOnlyCollection<Guid> ids);
        Task<ProjectLookUpDto> GetProjectByIdAsync(string projectId, CancellationToken cancellationToken = default);
    }
}