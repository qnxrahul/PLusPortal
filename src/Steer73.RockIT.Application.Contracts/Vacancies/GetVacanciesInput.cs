using Volo.Abp.Application.Dtos;
using System;
using Steer73.RockIT.Enums;
using System.Collections.Generic;

namespace Steer73.RockIT.Vacancies
{
    public abstract class GetVacanciesInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Title { get; set; }
        public string? Reference { get; set; }
        public Region? Region { get; set; }
        public string? Role { get; set; }
        public string? Benefits { get; set; }
        public string? Location { get; set; }
        public string? Salary { get; set; }
        public string? RoleType { get; set; }
        public string? Description { get; set; }
        public string? FormalInterviewDate { get; set; }
        public string? SecondInterviewDate { get; set; }
        public DateOnly? ExternalPostingDateMin { get; set; }
        public DateOnly? ExternalPostingDateMax { get; set; }
        public DateOnly? ClosingDateMin { get; set; }
        public DateOnly? ClosingDateMax { get; set; }
        public DateOnly? ExpiringDateMin { get; set; }
        public DateOnly? ExpiringDateMax { get; set; }
        public bool? ShowDiversity { get; set; }

        public bool? FlagHideVacancy { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? IdentityUserId { get; set; }
        public Guid? PracticeGroupId { get; set; }
        public Guid? VacancyFormDefinitionId { get; set; }
        public Guid? DiversityFormDefinitionId { get; set; }
        public VacancyStatus? Status { get; set; }
        public bool? ShowContributionVacancies { get; set; }

        public GetVacanciesInputBase()
        {

        }
    }
}