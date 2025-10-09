using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyContributorDto
    {
        public Guid VacancyId { get; set; }
        public Guid IdentityUserId { get; set; }

        public ContributorDto Contributor { get; set; } = null!;
    }
}
