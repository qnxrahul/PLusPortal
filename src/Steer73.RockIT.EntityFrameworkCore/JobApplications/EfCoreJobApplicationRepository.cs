using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.JobApplications
{
    public abstract class EfCoreJobApplicationRepositoryBase : EfCoreRepository<RockITDbContext, JobApplication, Guid>
    {
        public EfCoreJobApplicationRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<JobApplicationWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(jobApplication => new JobApplicationWithNavigationProperties
                {
                    JobApplication = jobApplication,
                    Vacancy = dbContext.Set<Vacancy>().FirstOrDefault(c => c.Id == jobApplication.VacancyId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<JobApplicationWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            Guid? vacancyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, emailAddress, title, phoneNumber, landline, currentRole, currentCompany, currentPositionType, vacancyId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? JobApplicationConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<JobApplicationWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from jobApplication in (await GetDbSetAsync())
                   join vacancy in (await GetDbContextAsync()).Set<Vacancy>() on jobApplication.VacancyId equals vacancy.Id into vacancies
                   from vacancy in vacancies.DefaultIfEmpty()
                   select new JobApplicationWithNavigationProperties
                   {
                       JobApplication = jobApplication,
                       Vacancy = vacancy
                   };
        }

        protected virtual IQueryable<JobApplicationWithNavigationProperties> ApplyFilter(
            IQueryable<JobApplicationWithNavigationProperties> query,
            string? filterText,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            Guid? vacancyId = null)
        {
            if (!string.IsNullOrWhiteSpace(filterText))
            {
                JobApplicationStatus status;
                if (Enum.TryParse(filterText, ignoreCase: true, out status))
                {
                    query = query.Where(e =>
                        e.JobApplication.FirstName!.Contains(filterText!)
                        || e.JobApplication.LastName!.Contains(filterText!)
                        || e.JobApplication.EmailAddress!.Contains(filterText!)
                        || e.JobApplication.Title!.Contains(filterText!)
                        || e.JobApplication.PhoneNumber!.Contains(filterText!)
                        || e.JobApplication.Landline!.Contains(filterText!)
                        || e.JobApplication.CurrentRole!.Contains(filterText!)
                        || e.JobApplication.CurrentCompany!.Contains(filterText!)
                        || e.JobApplication.CurrentPositionType!.Contains(filterText!)
                        || (e.Vacancy != null && (
                            e.Vacancy.Title!.Contains(filterText!)
                            || e.Vacancy.ProjectId!.Contains(filterText!)
                            || e.Vacancy.Role!.Contains(filterText!)
                        ))
                        || e.JobApplication.Status == status);
                }
                else
                {
                    query = query.Where(e =>
                        e.JobApplication.FirstName!.Contains(filterText!)
                        || e.JobApplication.LastName!.Contains(filterText!)
                        || e.JobApplication.EmailAddress!.Contains(filterText!)
                        || e.JobApplication.Title!.Contains(filterText!)
                        || e.JobApplication.PhoneNumber!.Contains(filterText!)
                        || e.JobApplication.Landline!.Contains(filterText!)
                        || e.JobApplication.CurrentRole!.Contains(filterText!)
                        || e.JobApplication.CurrentCompany!.Contains(filterText!)
                        || e.JobApplication.CurrentPositionType!.Contains(filterText!)
                        || (e.Vacancy != null && (
                            e.Vacancy.Title!.Contains(filterText!)
                            || e.Vacancy.ProjectId!.Contains(filterText!)
                            || e.Vacancy.Role!.Contains(filterText!)
                        ))
                    );
                }
            }

            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.JobApplication.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.JobApplication.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), e => e.JobApplication.EmailAddress.Contains(emailAddress))
                    .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.JobApplication.Title.Contains(title))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.JobApplication.PhoneNumber.Contains(phoneNumber))
                    .WhereIf(!string.IsNullOrWhiteSpace(landline), e => e.JobApplication.Landline.Contains(landline))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentRole), e => e.JobApplication.CurrentRole.Contains(currentRole))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentCompany), e => e.JobApplication.CurrentCompany.Contains(currentCompany))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentPositionType), e => e.JobApplication.CurrentPositionType.Contains(currentPositionType))
                    .WhereIf(vacancyId != null && vacancyId != Guid.Empty, e => e.Vacancy != null && e.Vacancy.Id == vacancyId);
        }

        public virtual async Task<List<JobApplication>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, firstName, lastName, emailAddress, title, phoneNumber, landline, currentRole, currentCompany, currentPositionType);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? JobApplicationConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null,
            Guid? vacancyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, emailAddress, title, phoneNumber, landline, currentRole, currentCompany, currentPositionType, vacancyId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<JobApplication> ApplyFilter(
            IQueryable<JobApplication> query,
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? emailAddress = null,
            string? title = null,
            string? phoneNumber = null,
            string? landline = null,
            string? currentRole = null,
            string? currentCompany = null,
            string? currentPositionType = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName!.Contains(filterText!) || e.LastName!.Contains(filterText!) || e.EmailAddress!.Contains(filterText!) || e.Title!.Contains(filterText!) || e.PhoneNumber!.Contains(filterText!) || e.Landline!.Contains(filterText!) || e.CurrentRole!.Contains(filterText!) || e.CurrentCompany!.Contains(filterText!) || e.CurrentPositionType!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), e => e.EmailAddress.Contains(emailAddress))
                    .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.Title.Contains(title))
                    .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), e => e.PhoneNumber.Contains(phoneNumber))
                    .WhereIf(!string.IsNullOrWhiteSpace(landline), e => e.Landline.Contains(landline))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentRole), e => e.CurrentRole.Contains(currentRole))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentCompany), e => e.CurrentCompany.Contains(currentCompany))
                    .WhereIf(!string.IsNullOrWhiteSpace(currentPositionType), e => e.CurrentPositionType.Contains(currentPositionType));
        }
    }
}