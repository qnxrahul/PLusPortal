using Steer73.RockIT.FormDefinitions;
using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class GetFormDefinitionsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? ReferenceId { get; set; }
        public string? Name { get; set; }
        public string? FormDetails { get; set; }
        public FormType? FormType { get; set; }
        public Guid? CompanyId { get; set; }

        public GetFormDefinitionsInputBase()
        {

        }
    }
}