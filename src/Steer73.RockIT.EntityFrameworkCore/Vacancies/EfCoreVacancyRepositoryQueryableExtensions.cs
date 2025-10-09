using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Steer73.RockIT.Vacancies;

public static class EfCoreVacancyRepositoryQueryableExtensions
{
    public static IQueryable<Vacancy> IncludeDetails(
        this IQueryable<Vacancy> queryable,
        bool include = true)
    {
        if (!include) { return queryable; }

        return queryable
            .Include("VacancyRegions")
            .Include("PracticeGroups")
            .Include("Contributors")
            .Include("Contributors.IdentityUser");
    }
}
