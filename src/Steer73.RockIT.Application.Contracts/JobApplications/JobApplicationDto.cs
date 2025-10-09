using System;
using System.Collections.Generic;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.JobFormResponses;
using Steer73.RockIT.DiversityFormResponses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Aka { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Landline { get; set; }
        public string? CurrentRole { get; set; }
        public string? CurrentCompany { get; set; }
        public string? CurrentPositionType { get; set; }
        public string? CVUrl { get; set; }
        public string? CoverLetterUrl { get; set; }
        public string? AdditionalDocumentUrl { get; set; }
        public string? ResponseUrl { get; set; }
        public JobApplicationStatus? Status { get; set; }
		public string? StatusAsString { get; set; }

        public Guid VacancyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public List<DiversityDataDto> DiversityDatas { get; set; } = new();
        public List<JobFormResponseDto> JobFormResponses { get; set; } = new();
        public List<DiversityFormResponseDto> DiversityFormResponses { get; set; } = new();
    }
}