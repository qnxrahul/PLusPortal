using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Steer73.RockIT.Shared;

namespace Steer73.RockIT.Companies
{
    public partial interface ICompaniesAppService : IApplicationService
    {

        Task<PagedResultDto<CompanyDto>> GetListAsync(GetCompaniesInput input);

        Task<CompanyDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<CompanyDto> CreateAsync(CompanyCreateDto input);

        Task<CompanyDto> UpdateAsync(Guid id, CompanyUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CompanyExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> companyIds);

        Task DeleteAllAsync(GetCompaniesInput input);
        Task<Steer73.RockIT.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
        Task<ListResultDto<CompanyAutoCompleteDto>> GetListForAutoCompleteAsync(string filter = "");

    }
}