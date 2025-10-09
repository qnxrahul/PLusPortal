using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Steer73.RockIT.Web.Pages.Shared
{
    public abstract class VacancyReferenceModalBase : RockITPageModel
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }

        public VacancyReferenceModalBase()
        {
            ProjectId = string.Empty;
            ProjectName = string.Empty;
            CompanyName = string.Empty;
        }

        public virtual Task OnGetAsync(int id, string projectId, string projectName, string companyName)
        {
            Id = id;
            ProjectId = projectId;
            ProjectName = projectName;
            CompanyName = companyName;

            return Task.CompletedTask;
        }
    }
}