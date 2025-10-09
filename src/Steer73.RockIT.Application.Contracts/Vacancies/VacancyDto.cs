using Steer73.RockIT.Companies;
using Steer73.RockIT.Enums;
using Steer73.RockIT.PracticeGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Title { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string MediaSources { get; set; } = null!;
        public string? Role { get; set; }
        public string? Benefits { get; set; }
        public string? Location { get; set; }
        public string? Salary { get; set; }
        public string RoleType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? FormalInterviewDate { get; set; }

        public bool Flag_HideVacancy { get; set; } 
        public string? SecondInterviewDate { get; set; }
        public DateOnly ExternalPostingDate { get; set; }
        public DateOnly ClosingDate { get; set; }
        public DateOnly ExpiringDate { get; set; }
        public Guid? BrochureFileId { get; set; }

        public DateTime? BrochureLastUpdatedAt { get; set; }
        public Guid? AdditionalFileId { get; set; }
        public bool ShowDiversity { get; set; }
        public Guid CompanyId { get; set; }
        public Guid IdentityUserId { get; set; }
        public Guid? VacancyFormDefinitionId { get; set; }
        public Guid? DiversityFormDefinitionId { get; set; }
        public string? LinkedInUrl { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
        public int? ExternalRefId { get; set; }
        public string? ProjectId { get; set; }

        public List<Region> Regions { get; set; } = [];

        public List<string> RegionDescriptions => [.. Regions
            .Select(r => r.GetDescription())
            .OrderBy(d => d)];

        public List<PracticeGroupDto> Groups { get; set; } = [];
        public List<VacancyContributorDto> Contributors { get; set; } = [];
        public CompanyDto? Company { get; set; }

        public List<string> GroupNames => [.. Groups
            .Select(pg => pg.Name)
            .OrderBy(n => n)];
    }
}