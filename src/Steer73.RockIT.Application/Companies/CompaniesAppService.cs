using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.Companies;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Steer73.RockIT.Shared;

namespace Steer73.RockIT.Companies
{

    [AllowAnonymous]
    public abstract class CompaniesAppServiceBase : RockITAppService
    {
        protected IDistributedCache<CompanyDownloadTokenCacheItem, string> _downloadTokenCache;
        protected ICompanyRepository _companyRepository;
        protected CompanyManager _companyManager;

        public CompaniesAppServiceBase(ICompanyRepository companyRepository, CompanyManager companyManager, IDistributedCache<CompanyDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _companyRepository = companyRepository;
            _companyManager = companyManager;

        }

        public virtual async Task<PagedResultDto<CompanyDto>> GetListAsync(GetCompaniesInput input)
        {
            var totalCount = await _companyRepository.GetCountAsync(input.FilterText, input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact);
            var items = await _companyRepository.GetListAsync(input.FilterText, input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CompanyDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Company>, List<CompanyDto>>(items)
            };
        }

        public virtual async Task<CompanyDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Company, CompanyDto>(await _companyRepository.GetAsync(id));
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _companyRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<CompanyDto> CreateAsync(CompanyCreateDto input)
        {

            var company = await _companyManager.CreateAsync(
            input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact, input.LogoUrl
            );

            return ObjectMapper.Map<Company, CompanyDto>(company);
        }

        [AllowAnonymous]
        public virtual async Task<CompanyDto> UpdateAsync(Guid id, CompanyUpdateDto input)
        {

            var company = await _companyManager.UpdateAsync(
            id,
            input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact, input.LogoUrl, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Company, CompanyDto>(company);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CompanyExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _companyRepository.GetListAsync(input.FilterText, input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Company>, List<CompanyExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Companies.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [AllowAnonymous]
        public virtual async Task DeleteByIdsAsync(List<Guid> companyIds)
        {
            await _companyRepository.DeleteManyAsync(companyIds);
        }

        [AllowAnonymous]
        public virtual async Task DeleteAllAsync(GetCompaniesInput input)
        {
            await _companyRepository.DeleteAllAsync(input.FilterText, input.Name, input.Phone, input.Address, input.Postcode, input.PrimaryContact);
        }
        public virtual async Task<Steer73.RockIT.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new CompanyDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new Steer73.RockIT.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
        public virtual async Task<ListResultDto<CompanyAutoCompleteDto>> GetListForAutoCompleteAsync(string filter = "")
        {
            var companies = await _companyRepository.GetListAsync(
                name: filter,
                maxResultCount: 15);

            var dtos = companies.Select(c => new CompanyAutoCompleteDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return new ListResultDto<CompanyAutoCompleteDto>(dtos);
        }
    }
}