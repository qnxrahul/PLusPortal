using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Steer73.RockIT.Shared;
using Steer73.RockIT.PracticeGroups;

namespace Steer73.RockIT.Vacancies
{
    public partial interface IVacanciesAppService : IApplicationService
    {
        Task<IRemoteStreamContent> GetFileAsync(GetFileInput input);

        Task<AppFileDescriptorDto> UploadFileAsync(IRemoteStreamContent input);

        Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetListAsync(GetVacanciesInput input);

        Task<VacancyWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<VacancyDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetCompanyLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetPracticeGroupLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<FormDefinitionLookup<Guid>>> GetFormDefinitionLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<VacancyDto> CreateAsync(VacancyCreateDto input);

        Task<VacancyDto> UpdateAsync(Guid id, VacancyUpdateDto input);
        Task<Steer73.RockIT.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
        Task<ListResultDto<VacancyUserAutoCompleteDto>> GetListForUserAutoCompleteAsync(string filter = "");
    }
}