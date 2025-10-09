using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.JobApplications
{
    public abstract class GetJobApplicationsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Landline { get; set; }
        public string? CurrentRole { get; set; }
        public string? CurrentCompany { get; set; }
        public string? CurrentPositionType { get; set; }
        public Guid? VacancyId { get; set; }

        public GetJobApplicationsInputBase()
        {

        }
    }
}