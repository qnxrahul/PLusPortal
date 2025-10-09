using EzekiaCRM;
using System;

namespace Steer73.RockIT.Vacancies
{
    public class ProjectLookUpDto
    {
        public EzekiaProjectDto? EzekiaProject { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
