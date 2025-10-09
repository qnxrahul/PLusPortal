using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? PrimaryContact { get; set; }

        public CompanyExcelDownloadDtoBase()
        {

        }
    }
}