using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyCreateDtoBase
    {
        [StringLength(CompanyConsts.NameMaxLength)]
        public string? Name { get; set; }
        [StringLength(CompanyConsts.PhoneMaxLength)]
        public string? Phone { get; set; }
        [StringLength(CompanyConsts.AddressMaxLength)]
        public string? Address { get; set; }
        [StringLength(CompanyConsts.PostcodeMaxLength)]
        public string? Postcode { get; set; }
        [StringLength(CompanyConsts.PrimaryContactMaxLength)]
        public string? PrimaryContact { get; set; }
        [StringLength(CompanyConsts.LogoUrlMaxLength)]
        public string? LogoUrl { get; set; }
    }
}