using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? PrimaryContact { get; set; }
        public string? LogoUrl { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}