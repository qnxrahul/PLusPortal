using System;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyExcelDtoBase
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? PrimaryContact { get; set; }
        public string? LogoUrl { get; set; }
    }
}