using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyUserAutoCompleteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
