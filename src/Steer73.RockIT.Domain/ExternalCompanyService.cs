using EzekiaCRM;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Steer73.RockIT.AppFileDescriptors;
using Steer73.RockIT.Companies;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;
using static Steer73.RockIT.Permissions.RockITSharedPermissions;
using Company = Steer73.RockIT.Companies.Company;
using Task = System.Threading.Tasks.Task;

namespace Steer73.RockIT.Domain.External
{
    public class ExternalCompanyService : DomainService, IExternalCompanyService
    {
        private const string ExternalDateFormat = "yyyy-MM-dd";

        private readonly ICompanyRepository _companyRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IClient _ezekiaClient;
        private readonly IAuditingManager _auditingManager;
        private readonly ILogger<ExternalCompanyService> _logger;
        private IRepository<AppFileDescriptor, Guid> _appFileDescriptorRepository;
        private IBlobContainer<JobApplicantContainer> _jobApplicantContainer;
        private IBlobContainer<VacancyFileContainer> _vacancyContainer;

        public ExternalCompanyService(
            ICompanyRepository companyRepository,
            IClient ezekiaClient,
            IAuditingManager auditingManager,
            IJobApplicationRepository jobApplicationRepository,
            IVacancyRepository vacancyRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ILogger<ExternalCompanyService> logger,
            IRepository<AppFileDescriptor, Guid> appFileDescriptorRepository,
            IBlobContainer<JobApplicantContainer> jobApplicantContainer,
            IBlobContainer<VacancyFileContainer> vacancyFileContainer)
        {
            _companyRepository = companyRepository;
            _ezekiaClient = ezekiaClient;
            _auditingManager = auditingManager;
            _jobApplicationRepository = jobApplicationRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
            _jobApplicantContainer = jobApplicantContainer;
            _vacancyRepository = vacancyRepository;
            _appFileDescriptorRepository = appFileDescriptorRepository;
            _vacancyContainer = vacancyFileContainer;
        }



        public async Task SendApplicantDataAsync(
    Guid jobApplicationId,
    CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _logger.LogInformation("SendApplicantDataAsync started for JobApplicationId: {JobApplicationId}", jobApplicationId);

            var jobApplication = await _jobApplicationRepository.GetAsync(jobApplicationId, cancellationToken: cancellationToken);
            _logger.LogInformation("Fetched JobApplication {JobApplicationId} from DB", jobApplicationId);

            var vacancy = await _vacancyRepository.GetAsync(jobApplication.VacancyId, cancellationToken: cancellationToken);
            _logger.LogInformation("Fetched Vacancy {VacancyId} from DB", vacancy.Id);

            var company = await _companyRepository.GetAsync(id: vacancy.CompanyId, cancellationToken: cancellationToken);
            _logger.LogInformation("Fetched Company {CompanyId} from DB", company.Id);

            var auditEntry = new AuditLogActionInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 100,
                ServiceName = "BackgroundWorker.SendApplicantData.Ezekia",
                MethodName = "SendApplicantData",
                Parameters = $"{{\"name\": \"{jobApplication.FirstName}\", \"surname\": \"{jobApplication.LastName}\", \"email\": \"{jobApplication.EmailAddress}\"}}"
            };

            auditEntry.ExtraProperties.Add(nameof(jobApplication.PhoneNumber), jobApplication.PhoneNumber ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.Landline), jobApplication.Landline ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(vacancy.Description), vacancy.Description ?? string.Empty);

            // NULL GUARDS: Add after the above.
            auditEntry.ExtraProperties.Add(nameof(company.ExternalRefId), company.ExternalRefId);
            auditEntry.ExtraProperties.Add(nameof(company.Name), company.Name ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.Title), jobApplication.Title ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.CurrentCompany), jobApplication.CurrentCompany ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.CurrentRole), jobApplication.CurrentRole ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.Aka), jobApplication.Aka ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(jobApplication.CurrentPositionType), jobApplication.CurrentPositionType ?? string.Empty);
            auditEntry.ExtraProperties.Add(nameof(vacancy.ExternalPostingDate), vacancy.ExternalPostingDate.ToString(ExternalDateFormat));
            auditEntry.ExtraProperties.Add(nameof(vacancy.ExpiringDate), vacancy.ExpiringDate.ToString(ExternalDateFormat));

            using (var auditingScope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.Url = CompanyConsts.BackgroundWorkerLogSendApplicantDataUrl;
                _auditingManager.Current.Log.HttpMethod = HttpMethod.Post.Method;
                _auditingManager.Current.Log.UserName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ClientName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ApplicationName = CompanyConsts.BackgroundWorkerLogApplicationName;
                _auditingManager.Current.Log.UserId = Guid.NewGuid();
                _auditingManager.Current.Log.CorrelationId = Guid.NewGuid().ToString();
                _auditingManager.Current.Log.Comments = [CompanyConsts.BackgroundWorkerLogEntryComment];
                _auditingManager.Current.Log.Actions.Add(auditEntry);

                var syncResult = false;
                int? ezekiaJobApplicationId = null;

                try
                {
                    _logger.LogInformation("Calling Ezekia APIs for email: {Email} and company: {Company}", jobApplication.EmailAddress, jobApplication.CurrentCompany);

                    var email = $"\"{jobApplication.EmailAddress}\"";

                    var peopleTask = _ezekiaClient.V3PeopleGetAsync(
                        query: email,
                        filterOn: [Anonymous6.Email],
                        sortBy: null,
                        fields: null,
                        exclude: null,
                        counts: null,
                        sortOrder: null,
                        page: null,
                        count: 1,
                        since: null,
                        before: null,
                        between: null,
                        view: null,
                        fuzzy: null,
                        tags: null,
                        isCandidate: true,
                        onCompanies: null, // Should this be true?
                        cancellationToken: cancellationToken);

                    var companiesTask = _ezekiaClient.CompaniesGetAsync(
                        query: jobApplication.CurrentCompany,
                        filterOn: [Anonymous.Name],
                        sortBy: null,
                        fields: null,
                        exclude: null,
                        counts: null,
                        sortOrder: null,
                        page: null,
                        count: 1,
                        from: null,
                        to: null,
                        since: null,
                        before: null,
                        between: null,
                        view: null,
                        withArchived: null,
                        archived: null,
                        fuzzy: null,
                        tags: null,
                        withPinned: null,
                        cancellationToken: cancellationToken);

                    await Task.WhenAll(peopleTask, companiesTask);
                    _logger.LogInformation("Completed Ezekia API calls for JobApplicationId: {JobApplicationId}", jobApplicationId);

                    var person = peopleTask.Result.Data.FirstOrDefault();
                    var jobApplicationCompanyId = companiesTask.Result.Data?.FirstOrDefault()?.Id;

                    if (!vacancy.ExternalRefId.HasValue)
                    {
                        _logger.LogWarning("Vacancy {VacancyId} has no ExternalRefId. Attempting sync before assigning candidate.", vacancy.Id);
                        await SendVacancyDataAsync(vacancy.Id, cancellationToken);
                        vacancy = await _vacancyRepository.GetAsync(vacancy.Id, cancellationToken: cancellationToken);
                        _logger.LogInformation("Vacancy {VacancyId} ExternalRefId updated to {ExternalRefId}.", vacancy.Id, vacancy.ExternalRefId);

                    }

                    if (person is null)
                    {
                        _logger.LogInformation("Creating person in Ezekia for JobApplicationId: {JobApplicationId}", jobApplicationId);
                        ezekiaJobApplicationId = await CreatePerson(jobApplication, vacancy, jobApplicationCompanyId, cancellationToken);


                        if (ezekiaJobApplicationId.HasValue)
                        {
                            // After creating, immediately fetch the person and update it.
                            _logger.LogInformation("Successfully created person in Ezekia with PersonId: {PersonId} for JobApplicationId: {JobApplicationId}", ezekiaJobApplicationId.Value, jobApplicationId);
                            _logger.LogInformation("Fetching newly created person from Ezekia for PersonId: {PersonId}", ezekiaJobApplicationId.Value);

                            var createdPerson = await _ezekiaClient.V3PeopleGetAsync(null, ezekiaJobApplicationId.Value, cancellationToken);
                            if (createdPerson?.Data != null)
                            {
                                _logger.LogInformation("Fetched person successfully from Ezekia. Proceeding to UpdatePerson for PersonId: {PersonId}", createdPerson.Data.Id);
                                await UpdatePerson(createdPerson.Data, jobApplication, vacancy, jobApplicationCompanyId, cancellationToken);
                                _logger.LogInformation("Completed UpdatePerson for newly created PersonId: {PersonId}", createdPerson.Data.Id);
                            }
                            else
                            {
                                _logger.LogWarning("Failed to retrieve newly created person from Ezekia for PersonId: {PersonId}", ezekiaJobApplicationId.Value);
                            }
                        }

                        else
                        {
                            _logger.LogWarning("Failed to create person in Ezekia for JobApplicationId: {JobApplicationId}", jobApplicationId);
                        }

                    }
                    else
                    {
                        _logger.LogInformation("Person already exists in Ezekia with PersonId: {PersonId} for JobApplicationId: {JobApplicationId}", person.Id, jobApplicationId);
                        ezekiaJobApplicationId = person.Id;
                        await UpdatePerson(person, jobApplication, vacancy, jobApplicationCompanyId, cancellationToken);
                        _logger.LogInformation("Completed UpdatePerson for existing PersonId: {PersonId}", person.Id);
                    }

                    stopWatch.Stop();
                    auditEntry.ExecutionDuration = (int)stopWatch.Elapsed.TotalMilliseconds;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.OK;

                    syncResult = true;

                    _logger.LogInformation("Successfully sent JobApplication:{JobApplicationId} data into Ezekia", jobApplicationId);
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    _logger.LogError(ex, "Error occurred while sending JobApplication:{JobApplicationId} data into Ezekia", jobApplicationId);

                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager?.Current?.Log.Exceptions.Add(ex);

                    throw;
                }
                finally
                {
                    jobApplication.SyncStatusUpdate = DateTime.UtcNow;
                    jobApplication.SyncStatus = syncResult ? Enums.SyncStatus.Synced : Enums.SyncStatus.Error;
                    jobApplication.ExternalRefId = ezekiaJobApplicationId;

                    await _jobApplicationRepository.UpdateAsync(entity: jobApplication, cancellationToken: cancellationToken);
                    _logger.LogInformation("Updated JobApplication:{JobApplicationId} sync status as {SyncStatus}", jobApplicationId, jobApplication.SyncStatus);

                    await auditingScope.SaveAsync();
                }
            }
        }
        public async Task UpdateCompaniesAsync(
            int resultsPerPage = 50,
            int maxIterations = 100000,
            CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            var operationStartTime = DateTime.UtcNow;
            var auditEntry = new AuditLogActionInfo
            {
                ExecutionDuration = (int)stopWatch.ElapsedMilliseconds,
                ServiceName = "BackgroundWorker.UploadCompanies.Ezekia",
                MethodName = "UpdateCompanies",
                Parameters = $"{{\"ResultsPerPage\": \"{resultsPerPage}\"}}"
            };

            stopWatch.Start();
            using var uow = _unitOfWorkManager.Begin();
            using (var auditingScope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.Url = CompanyConsts.BackgroundWorkerLogUploadCompaniesUrl;
                _auditingManager.Current.Log.HttpMethod = HttpMethod.Get.Method;
                _auditingManager.Current.Log.UserName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ClientName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ApplicationName = CompanyConsts.BackgroundWorkerLogApplicationName;
                _auditingManager.Current.Log.UserId = Guid.NewGuid();
                _auditingManager.Current.Log.CorrelationId = Guid.NewGuid().ToString();
                _auditingManager.Current.Log.Comments = [CompanyConsts.BackgroundWorkerLogEntryComment];
                _auditingManager.Current.Log.Actions.Add(auditEntry);

                try
                {
                    // Get id of first company
                    var firstCompanyTask = _ezekiaClient.CompaniesGetAsync(
                        query: null,
                        filterOn: null,
                        sortBy: SortBy.Id,
                        fields: null,
                        exclude: null,
                        counts: null,
                        sortOrder: SortOrder.Asc,
                        page: null,
                        count: 1,
                        from: null,
                        to: null,
                        since: null,
                        before: null,
                        between: null,
                        view: null,
                        withArchived: null,
                        archived: null,
                        fuzzy: null,
                        tags: null,
                        withPinned: null,
                        cancellationToken: cancellationToken);

                    // Get id of last company
                    var lastCompanyTask = _ezekiaClient.CompaniesGetAsync(
                        query: null,
                        filterOn: null,
                        sortBy: SortBy.Id,
                        fields: null,
                        exclude: null,
                        counts: null,
                        sortOrder: SortOrder.Desc,
                        page: null,
                        count: 1,
                        from: null,
                        to: null,
                        since: null,
                        before: null,
                        between: null,
                        view: null,
                        withArchived: null,
                        archived: null,
                        fuzzy: null,
                        tags: null,
                        withPinned: null,
                        cancellationToken: cancellationToken);

                    await Task.WhenAll(
                        firstCompanyTask,
                        lastCompanyTask);

                    var from = (firstCompanyTask.Result.Data.FirstOrDefault()?.Id ?? 0) - 1;
                    var lastId = lastCompanyTask.Result.Data.FirstOrDefault()?.Id ?? 0;

                    _logger.LogInformation("Fetching companies from {from} to {lastId}", from, lastId);

                    var totalCompaniesAdded = 0;
                    var totalCompaniesUpdated = 0;
                    var totalCompaniesProcessed = 0;

                    var iteration = 0;

                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (++iteration > maxIterations)
                        {
                            _logger.LogWarning("Max iterations ({maxIterations}) reached in UpdateCompaniesAsync. Breaking loop to avoid infinite processing.", maxIterations);
                            break;
                        }

                        Response2 companiesResult = null;
                        try
                        {
                            companiesResult = await _ezekiaClient.CompaniesGetAsync(
                                query: null,
                                filterOn: null,
                                sortBy: SortBy.Id,
                                fields: null,
                                exclude: null,
                                counts: null,
                                sortOrder: SortOrder.Asc,
                                page: null,
                                count: resultsPerPage,
                                from: from, // value specified is non-inclusive
                                to: null,
                                since: null,
                                before: null,
                                between: null,
                                view: null,
                                withArchived: null,
                                archived: null,
                                fuzzy: null,
                                tags: null,
                                withPinned: null,
                                cancellationToken: cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to fetch companies from Ezekia for 'from'={from}. Skipping to next batch.", from);
                            // Move 'from' forward to avoid infinite loop
                            from += resultsPerPage;
                            continue;
                        }

                        var companies = companiesResult.Data;
                        _logger.LogInformation("Fetched {companiesFetched} companies from Ezekia", companies.Count);

                        if (companies.Count == 0)
                        {
                            break;
                        }

                        var externalRefIds = companies.Select(c => c.Id).ToList();
                        var existingCompanies = externalRefIds.Count > 0 ?
                            (await _companyRepository.GetByExternalRefIds(externalRefIds, cancellationToken))
                            .ToDictionary(c => (int)c.ExternalRefId!) : [];
                        var companiesToAdd = new List<Company>();
                        var companiesToUpdate = new List<Company>();

                        foreach (var item in companies)
                        {
                            try
                            {
                                var addressDetails = new string[]
                                {
                                    item.Address?.Line1!,
                                    item.Address?.Line2!,
                                    item.Address?.Line3!,
                                    item.Address?.City!,
                                    item.Address?.State!,
                                    item.Address?.Country!,
                                    item.Address?.Postcode!
                                };
                                var addressFull = string.Join(" ", addressDetails.Where(x => !string.IsNullOrWhiteSpace(x)));

                                if (existingCompanies.TryGetValue(item!.Id, out Company? company))
                                {
                                    company.Name = item.Name;
                                    company.Phone = item.Phone;
                                    company.Address = item.Address?.Full ?? addressFull;
                                    company.Postcode = item.Address?.Postcode;
                                    company.PrimaryContact = item.Email;
                                    company.LogoUrl = item.Image?.Url;
                                    companiesToUpdate.Add(company);
                                }
                                else
                                {
                                    companiesToAdd.Add(new Company(
                                        Guid.NewGuid(),
                                        name: item.Name,
                                        phone: item.Phone,
                                        address: item.Address?.Full ?? addressFull,
                                        postcode: item.Address?.Postcode,
                                        primaryContact: item.Email,
                                        logoUrl: item.Image?.Url,
                                        externalRefId: item.Id));
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to process company Id:{companyId}, Name:{companyName}", item?.Id, item?.Name);
                            }
                        }

                        try
                        {
                            if (companiesToAdd.Count > 0)
                            {
                                await _companyRepository.InsertManyAsync(
                                    companiesToAdd,
                                    cancellationToken: cancellationToken);
                            }

                            if (companiesToUpdate.Count > 0)
                            {
                                await _companyRepository.UpdateManyAsync(
                                    companiesToUpdate,
                                    cancellationToken: cancellationToken);
                            }

                            if (companiesToAdd.Count > 0 || companiesToUpdate.Count > 0)
                            {
                                await uow.SaveChangesAsync(cancellationToken);
                            }

                            _logger.LogInformation(
                                "Successfully added {companiesAdded} and updated {companiesUpdated} companies from Ezekia",
                                companiesToAdd.Count,
                                companiesToUpdate.Count);

                            totalCompaniesAdded += companiesToAdd.Count;
                            totalCompaniesUpdated += companiesToUpdate.Count;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to insert/update batch of companies.");
                            // Continue to next batch
                        }

                        totalCompaniesProcessed += companies.Count;

                        // Set from to the last company's Id for the next page
                        var lastCompany = companies.LastOrDefault();
                        if (lastCompany == null || lastCompany.Id >= lastId)
                        {
                            break;
                        }
                        from = lastCompany.Id;
                    }

                    stopWatch.Stop();

                    auditEntry.ExecutionDuration = (int)stopWatch.Elapsed.TotalMilliseconds;

                    auditEntry.ExtraProperties.Add("TotalResultsReceived", totalCompaniesProcessed.ToString());
                    auditEntry.ExtraProperties.Add("AddedCompaniesCount", totalCompaniesAdded.ToString());
                    auditEntry.ExtraProperties.Add("UpdatedCompaniesCount", totalCompaniesUpdated.ToString());
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.OK;

                    _logger.LogInformation("Successfully finished companies update from Ezekia");
                }
                catch (Exception ex)
                {
                    //Add exceptions
                    _logger.LogException(ex);
                    stopWatch.Stop();

                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager?.Current?.Log.Exceptions.Add(ex);

                    _logger.LogInformation("Failed to obtain companies from Ezekia");

                    throw;
                }
                finally
                {
                    //Always save the log
                    await auditingScope.SaveAsync();
                }
            }
        }

        public async Task SendVacancyDataAsync(Guid vacancyId, CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            var auditEntry = new AuditLogActionInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 100,
                ServiceName = "BackgroundWorker.SendVacancyData.Ezekia",
                MethodName = "SendVacancyData",
                Parameters = $"{{\"vacancyId\": \"{vacancyId}\"}}"
            };

            stopWatch.Start();

            using (var auditingScope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.Url = CompanyConsts.BackgroundWorkerLogSendVacancyDataUrl;
                _auditingManager.Current.Log.HttpMethod = HttpMethod.Post.Method;
                _auditingManager.Current.Log.UserName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ClientName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ApplicationName = CompanyConsts.BackgroundWorkerLogApplicationName;
                _auditingManager.Current.Log.UserId = Guid.NewGuid();
                _auditingManager.Current.Log.CorrelationId = Guid.NewGuid().ToString();
                _auditingManager.Current.Log.Comments = [CompanyConsts.BackgroundWorkerLogEntryComment];
                _auditingManager.Current.Log.Actions.Add(auditEntry);

                var syncResult = false;
                int? ezekiaVacancyId = null;
                var vacancy = await _vacancyRepository.GetAsync(vacancyId, cancellationToken: cancellationToken);
                var company = await _companyRepository.GetAsync(vacancy.CompanyId, cancellationToken: cancellationToken);
                IEnumerable<field4>? fields = null;

                try
                {
                    bool needsCreate = false;

                    if (vacancy.ExternalRefId.HasValue)
                    {
                        try
                        {
                            _logger.LogInformation("Checking Ezekia for existing ExternalRefId {ExternalRefId}", vacancy.ExternalRefId);
                            await _ezekiaClient.V3ProjectsGetAsync(vacancy.ExternalRefId.Value, cancellationToken);
                        }
                        catch (EzekiaCRM.ApiException ex) when (ex.StatusCode == 404)
                        {
                            _logger.LogWarning("Vacancy ExternalRefId {ExternalRefId} not found in Ezekia (404). Resetting.", vacancy.ExternalRefId);
                            vacancy.ExternalRefId = null;
                            await _vacancyRepository.UpdateAsync(vacancy, cancellationToken: cancellationToken);
                            needsCreate = true;
                        }
                    }
                    else
                    {
                        needsCreate = true;
                    }

                    if (needsCreate)
                    {
                        _logger.LogInformation("Creating new vacancy for {VacancyId} in Ezekia", vacancyId);
                        var createRequest = new store2
                        {
                            _type = Store2_type.Info,
                            Label = Store2Label.Assignment,
                            Name = vacancy.Title,
                            CompanyId = company.ExternalRefId?.ToString(),
                            Description = vacancy.Description,
                            StartDate = vacancy.ExternalPostingDate.ToString(ExternalDateFormat),
                            EndDate = vacancy.ExpiringDate.ToString(ExternalDateFormat)
                        };

                        var createResponse = await _ezekiaClient.ProjectsPostAsync(null, createRequest, cancellationToken);
                        ezekiaVacancyId = createResponse.Data.Id;
                        vacancy.ExternalRefId = ezekiaVacancyId;
                    }
                    else
                    {
                        _logger.LogInformation("Updating existing vacancy {VacancyId} in Ezekia", vacancyId);
                        var updateRequest = new store2
                        {
                            _type = Store2_type.Info,
                            Label = Store2Label.Assignment,
                            Name = vacancy.Title,
                            CompanyId = company.ExternalRefId?.ToString(),
                            Description = vacancy.Description,
                            StartDate = vacancy.ExternalPostingDate.ToString(ExternalDateFormat),
                            EndDate = vacancy.ExpiringDate.ToString(ExternalDateFormat)
                        };

                        var updateResponse = await _ezekiaClient.ProjectsPutAsync(fields, vacancy.ExternalRefId!.Value, updateRequest, cancellationToken);
                        ezekiaVacancyId = updateResponse.Data.Id;
                    }

                    stopWatch.Stop();

                    auditEntry.ExecutionDuration = (int)stopWatch.Elapsed.TotalMilliseconds;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.OK;

                    syncResult = true;
                    _logger.LogInformation("Successfully synced Vacancy:{VacancyId} to Ezekia", vacancyId);
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    _logger.LogException(ex);
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.Exceptions.Add(ex);
                    _logger.LogInformation("Error when sending Vacancy:{VacancyId} data into Ezekia", vacancyId);
                    throw;
                }
                finally
                {
                    vacancy.SyncStatusUpdate = DateTime.UtcNow;
                    vacancy.SyncStatus = syncResult ? Enums.SyncStatus.Synced : Enums.SyncStatus.Error;

                    await _vacancyRepository.UpdateAsync(vacancy, cancellationToken: cancellationToken);
                    _logger.LogInformation("Updated Vacancy:{VacancyId} sync status as {SyncStatus}", vacancyId, vacancy.SyncStatus);
                    await auditingScope.SaveAsync();
                }
            }
        }
        public async Task SendVacancyDocuments(
            Guid vacancyId,
            bool shouldUpdateAdditionalFile,
            bool shouldUpdateBrochure,
            CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            var operationStartTime = DateTime.UtcNow;
            var auditEntry = new AuditLogActionInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 100,
                ServiceName = "BackgroundWorker.SendVacancyDocuments.Ezekia",
                MethodName = "SendVacancyDocuments",
                Parameters = $"{{\"vacancyId\": \"{vacancyId}\"}}"
            };

            stopWatch.Start();

            using (var auditingScope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.Url = CompanyConsts.BackgroundWorkerLogSendVacancyDataUrl;
                _auditingManager.Current.Log.HttpMethod = HttpMethod.Post.Method;
                _auditingManager.Current.Log.UserName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ClientName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ApplicationName = CompanyConsts.BackgroundWorkerLogApplicationName;
                _auditingManager.Current.Log.UserId = Guid.NewGuid();
                _auditingManager.Current.Log.CorrelationId = new Guid().ToString();
                _auditingManager.Current.Log.Comments = [CompanyConsts.BackgroundWorkerLogEntryComment];
                _auditingManager.Current.Log.Actions.Add(auditEntry);

                var vacancy = await _vacancyRepository.GetAsync(vacancyId, cancellationToken: cancellationToken);
                IReadOnlyCollection<DocumentDto> documents = [];

                try
                {
                    documents = await GetVacancyDocuments(
                       vacancy,
                       shouldUpdateAdditionalFile,
                       shouldUpdateBrochure,
                       cancellationToken);
                    if (documents.Count > 0)
                    {
                        await _ezekiaClient.UploadDocuments(
                           Type5.Assignments,
                           vacancy.ExternalRefId!.Value.ToString(), // the job will fail if ExternalRefId is null
                           documents,
                           cancellationToken);
                    }

                    stopWatch.Stop();

                    auditEntry.ExecutionDuration = (int)stopWatch.Elapsed.TotalMilliseconds;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.OK;

                    _logger.LogInformation("Successfully sent documents for vacancy:{vacancyId} to Ezekia", vacancyId);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                    //Add exceptions
                    stopWatch.Stop();

                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager?.Current?.Log.Exceptions.Add(ex);

                    var fileNames = string.Join(", ", documents.Select(d => d.FileName).ToArray());
                    _logger.LogInformation("Error when sending documents:{fileNames} for vacancy:{vacancyId} to Ezekia", fileNames, vacancyId);

                    throw;
                }
                finally
                {
                    await auditingScope.SaveAsync();
                }
            }
        }

        public async Task SendApplicantDocuments(
            Guid jobApplicationId,
            CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            var operationStartTime = DateTime.UtcNow;
            var auditEntry = new AuditLogActionInfo
            {
                ExecutionTime = DateTime.UtcNow,
                ExecutionDuration = 100,
                ServiceName = "BackgroundWorker.SendApplicantDocuments.Ezekia",
                MethodName = "SendApplicantDocuments",
                Parameters = $"{{\"jonApplicantionId\": \"{jobApplicationId}\"}}"
            };

            stopWatch.Start();

            using (var auditingScope = _auditingManager.BeginScope())
            {
                _auditingManager.Current.Log.Url = CompanyConsts.BackgroundWorkerLogSendApplicantDocumentsUrl;
                _auditingManager.Current.Log.HttpMethod = HttpMethod.Post.Method;
                _auditingManager.Current.Log.UserName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ClientName = CompanyConsts.BackgroundWorkerLogUserName;
                _auditingManager.Current.Log.ApplicationName = CompanyConsts.BackgroundWorkerLogApplicationName;
                _auditingManager.Current.Log.UserId = Guid.NewGuid();
                _auditingManager.Current.Log.CorrelationId = new Guid().ToString();
                _auditingManager.Current.Log.Comments = [CompanyConsts.BackgroundWorkerLogEntryComment];
                _auditingManager.Current.Log.Actions.Add(auditEntry);

                var jobApplication = await _jobApplicationRepository.GetAsync(jobApplicationId, cancellationToken: cancellationToken);
                var vacancy = await _vacancyRepository.GetAsync(jobApplication.VacancyId, cancellationToken: cancellationToken);

                IReadOnlyCollection<DocumentDto> documents = [];

                try
                {
                    documents = await GetJobApplicationDocuments(
                        jobApplication,
                        vacancy.ExternalRefId!.Value,
                        cancellationToken);
                    if (documents.Count > 0)
                    {
                        await _ezekiaClient.UploadDocuments(
                            Type5.People,
                            jobApplication.ExternalRefId!.Value.ToString(), // the job will fail if ExternalRefId is null
                            documents,
                            cancellationToken);
                    }

                    stopWatch.Stop();

                    auditEntry.ExecutionDuration = (int)stopWatch.Elapsed.TotalMilliseconds;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.OK;

                    _logger.LogInformation("Successfully sent documents for job application:{jobApplicationId} to Ezekia", jobApplicationId);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                    //Add exceptions
                    stopWatch.Stop();

                    _auditingManager.Current.Log.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    _auditingManager.Current.Log.ExecutionDuration = (int)stopWatch.ElapsedMilliseconds;
                    _auditingManager?.Current?.Log.Exceptions.Add(ex);

                    var fileNames = string.Join(", ", documents.Select(d => d.FileName).ToArray());
                    _logger.LogInformation("Error when sending documents:{fileNames} for job application:{jobApplicationId} to Ezekia", fileNames, jobApplicationId);

                    throw;
                }
                finally
                {
                    await auditingScope.SaveAsync();
                }
            }
        }

        private async Task<int> CreatePerson(
            JobApplication jobApplication, 
            Vacancy vacancy,
            int? ezekiaCompanyId, 
            CancellationToken cancellationToken)
        {
            var phonesRequest = new List<phones4>();
            if (!string.IsNullOrWhiteSpace(jobApplication.PhoneNumber))
            {
                phonesRequest.Add(new phones4
                {
                    Number = jobApplication.PhoneNumber,
                    Label = Phones4Label.Mobile,
                    IsDefault = true
                });
            }
            if (!string.IsNullOrWhiteSpace(jobApplication.Landline)
                && !string.Equals(jobApplication.PhoneNumber, jobApplication.Landline, StringComparison.OrdinalIgnoreCase))
            {
                phonesRequest.Add(new phones4
                {
                    Number = jobApplication.Landline,
                    Label = Phones4Label.Home,
                    IsDefault = false
                });
            }
            var request = new store6Extended
            {
                Honorific = jobApplication.Title,
                FirstName = jobApplication.FirstName,
                LastName = jobApplication.LastName,
                Phones = phonesRequest,
                Emails =
                [
                    new()
                    {
                        Address = jobApplication.EmailAddress,
                        IsDefault = true,
                        Label = Emails4Label.Work
                    }
                ],
                Title = jobApplication.CurrentRole,
                Summary = jobApplication.CurrentRole,
                CompanyRecordId = ezekiaCompanyId is not null ? ezekiaCompanyId.Value : null,
                CompanyRecordName = ezekiaCompanyId is null ? jobApplication.CurrentCompany : null
            };

            if (!string.IsNullOrWhiteSpace(jobApplication.Aka))
            {
                request.Aliases = [jobApplication.Aka];
            }

            request.PositionType = Store6PositionType.Other;
            if (!string.IsNullOrWhiteSpace(jobApplication.CurrentPositionType)
                && Enum.TryParse(jobApplication.CurrentPositionType, true, out Store6PositionType positionType))
            {
                request.PositionType = positionType;
            }

            request.StartDate = vacancy.ExternalPostingDate.ToString(ExternalDateFormat);
            request.EndDate = vacancy.ExpiringDate.ToString(ExternalDateFormat);
            if (vacancy.ExternalRefId.HasValue)
            {
                request.Projects =
                [
                    new()            
                    {               
                        ProjectId =vacancy.ExternalRefId.Value,
                        ProjectName = vacancy.Title,            
                        ProjectType = ProjectType.Assignment            
                    }
                ];
            }
            _logger.LogInformation("Sending body to Ezekia: {0}", JsonConvert.SerializeObject(request),vacancy.ProjectId);

            var response = await _ezekiaClient.V3PeoplePostAsync(
                null, 
                request, 
                cancellationToken);


            var createdPersonId = response.Data.Id;


            // ✅ Step 2: Update candidate status
            if (vacancy.ExternalRefId.HasValue && createdPersonId > 0)
            {
                var statusRequest = new PipelineUpdateRequest
                {
                    // You may need to set one or more fields here like:
                   Candidates = [createdPersonId], // or "applied", etc. depending on Ezekia's enum
                                                   // Optionally: Date, Comments, or other metadata
                    Tags = [25198]
                };

                _logger.LogInformation("Updating candidate status in Ezekia for PersonId: {PersonId} on ProjectId: {ProjectId}", createdPersonId, vacancy.ExternalRefId.Value);

                await _ezekiaClient.V3ProjectsCandidatesStatusesPostAsync(
                    id: vacancy.ExternalRefId.Value,
                    
                    body: statusRequest,
                    cancellationToken: cancellationToken
                );

                _logger.LogInformation("Updated candidate status in Ezekia for PersonId: {PersonId}", createdPersonId);
            }

            return createdPersonId;

        }
        private async Task UpdatePerson(
        person2 person,
        JobApplication jobApplication,
        Vacancy vacancy,
        int? ezekiaCompanyId,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting UpdatePerson for JobApplicationId: {JobApplicationId}, PersonId: {PersonId}", jobApplication.Id, person.Id);

            var tasks = new List<Task>
            {
                _ezekiaClient.V3PeoplePutAsync(
            null,
            person.Id,
            new store6Extended
            {
                Honorific = jobApplication.Title,
                FirstName = jobApplication.FirstName,
                LastName = jobApplication.LastName
            },
            cancellationToken)
    };
            _logger.LogInformation("Queued V3PeoplePutAsync for PersonId: {PersonId}", person.Id);

            if (vacancy.ExternalRefId.HasValue)
            {
                try
                {
                    _logger.LogInformation("Assigning person {PersonId} to vacancy {VacancyId} and {VacancyTitle}", person.Id, vacancy.ExternalRefId.Value, vacancy.Title);
                    tasks.Add(_ezekiaClient.V3ProjectsCandidatesPostAsync(
                    new candidatesUpdateRequest2
                    {
                        Candidates = [person.Id],
                        ProjectId=vacancy.ExternalRefId.Value,
                        ProjectName=vacancy.Title,
                        Label=CandidatesUpdateRequest2Label.Assignment
                    },
                    cancellationToken));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Failed to assign person {PersonId} to vacancy {VacancyId}", person.Id, vacancy.ExternalRefId.Value);
                }
            }

            var positionType = StoreRequest11PositionType.Other;
            if (!string.IsNullOrWhiteSpace(jobApplication.CurrentPositionType))
            {
                Enum.TryParse(jobApplication.CurrentPositionType, true, out positionType);
            }

            if (!string.IsNullOrWhiteSpace(jobApplication.CurrentRole)
                && (person.Profile is null
                || person.Profile.Positions is null
                || !person.Profile.Positions.Any(
                    p => p.Company is not null
                         && (p.Company.Id == ezekiaCompanyId || p.Company.Name.Equals(jobApplication.CurrentCompany, StringComparison.OrdinalIgnoreCase))
                         && p.Title.Equals(jobApplication.CurrentRole, StringComparison.InvariantCultureIgnoreCase)
                         && p.PositionType.ToString().Equals(positionType.ToString(), StringComparison.InvariantCultureIgnoreCase))))
            {
                _logger.LogInformation("Adding new position for PersonId: {PersonId}, CompanyId: {CompanyId}", person.Id, ezekiaCompanyId);
                tasks.Add(_ezekiaClient.V2PeoplePositionsPostAsync(
                    person.Id,
                    new storeRequest11
                    {
                        CompanyRecordId = ezekiaCompanyId is not null ? ezekiaCompanyId.Value : null,
                        CompanyRecordName = ezekiaCompanyId is null ? jobApplication.CurrentCompany : null,
                        PositionType = positionType,
                        Summary = jobApplication.CurrentRole,
                        Title = jobApplication.CurrentRole,
                    },
                    cancellationToken));
            }

            if (!string.IsNullOrWhiteSpace(jobApplication.PhoneNumber)
                && (person.Phones is null
                    || !person.Phones.Any(p => p.Number.Equals(jobApplication.PhoneNumber, StringComparison.OrdinalIgnoreCase))))
            {
                _logger.LogInformation("Adding phone number for PersonId: {PersonId}", person.Id);
                tasks.Add(_ezekiaClient.PeoplePhonesPostAsync(
                    person.Id,
                    new phones2
                    {
                        IsDefault = true,
                        Label = Phones2Label.Mobile,
                        Number = jobApplication.PhoneNumber
                    },
                    cancellationToken));
            }

            if (!string.IsNullOrWhiteSpace(jobApplication.Landline)
                && !jobApplication.Landline.Equals(jobApplication.PhoneNumber, StringComparison.OrdinalIgnoreCase)
                && (person.Phones is null
                    || !person.Phones.Any(p => p.Number.Equals(jobApplication.Landline, StringComparison.OrdinalIgnoreCase))))
            {
                _logger.LogInformation("Adding landline number for PersonId: {PersonId}", person.Id);
                tasks.Add(_ezekiaClient.PeoplePhonesPostAsync(
                    person.Id,
                    new phones2
                    {
                        IsDefault = false,
                        Label = Phones2Label.Home,
                        Number = jobApplication.Landline
                    },
                    cancellationToken));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Completed UpdatePerson for JobApplicationId: {JobApplicationId}, PersonId: {PersonId}", jobApplication.Id, person.Id);
        }

        private async Task<IReadOnlyCollection<DocumentDto>> GetVacancyDocuments(
            Vacancy vacancy,
            bool shouldUpdateAdditionalFile,
            bool shouldUpdateBrochure,
            CancellationToken cancellationToken)
        {
            var documents = new List<DocumentDto>();
            var projectLabel = string.IsNullOrWhiteSpace(vacancy.ProjectId)
                ? ezekiaVacancyId.ToString()
                : vacancy.ProjectId;

            if (vacancy.BrochureFileId != null
                && shouldUpdateBrochure)
            {
                var fileDescriptor = await _appFileDescriptorRepository.GetAsync(
                    (Guid)vacancy.BrochureFileId,
                    cancellationToken: cancellationToken);

                if (fileDescriptor != null)
                {
                    var stream = await _vacancyContainer.GetAsync(
                        fileDescriptor.Id.ToString("N"),
                        cancellationToken);
                    if (stream != null)
                    {
                        var extension = Path.GetExtension(fileDescriptor.Name);
                        documents.Add(new DocumentDto
                        {
                            ContentType = FileUtils.GetContentType(extension),
                            FileName = $"Brochure{extension}",
                            Stream = stream
                        });
                    }
                }
            }

            if (vacancy.AdditionalFileId != null
                && shouldUpdateAdditionalFile)
            {
                var fileDescriptor = await _appFileDescriptorRepository.GetAsync(
                    (Guid)vacancy.AdditionalFileId,
                    cancellationToken: cancellationToken);
                if (fileDescriptor != null)
                {
                    var stream = await _vacancyContainer.GetAsync(
                        fileDescriptor.Id.ToString("N"),
                        cancellationToken);
                    if (stream != null)
                    {
                        var extension = Path.GetExtension(fileDescriptor.Name);
                        documents.Add(new DocumentDto
                        {
                            ContentType = FileUtils.GetContentType(extension),
                            FileName = $"AdditionalFile{extension}",
                            Stream = stream
                        });
                    }
                }
            }

            return documents;
        }

        private async Task<List<DocumentDto>> GetJobApplicationDocuments(
            JobApplication jobApplication,
            int ezekiaVacancyId,
            CancellationToken cancellationToken)
        {
            var documents = new List<DocumentDto>();
            char initial = !string.IsNullOrWhiteSpace(jobApplication.FirstName) ? 
                jobApplication.FirstName[0] : ' ';

            if (!string.IsNullOrWhiteSpace(jobApplication.CVUrl))
            {
                var stream = await _jobApplicantContainer.GetAsync(jobApplication.CVUrl, cancellationToken);
                if (stream != null)
                {
                    var extension = Path.GetExtension(jobApplication.CVUrl);
                    documents.Add(new DocumentDto
                    {
                        ContentType = FileUtils.GetContentType(extension),
                        FileName = $"{jobApplication.LastName}.{initial}.CV Original({projectLabel}){extension}",
                        Stream = stream
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(jobApplication.CoverLetterUrl))
            {
                var stream = await _jobApplicantContainer.GetAsync(jobApplication.CoverLetterUrl, cancellationToken);
                if (stream != null)
                {
                    var extension = Path.GetExtension(jobApplication.CoverLetterUrl);
                    documents.Add(new DocumentDto
                    {
                        ContentType = FileUtils.GetContentType(extension),
                        FileName = $"{jobApplication.LastName}.{initial}.Letter({projectLabel}){extension}",
                        Stream = stream
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(jobApplication.AdditionalDocumentUrl))
            {
                var stream = await _jobApplicantContainer.GetAsync(jobApplication.AdditionalDocumentUrl, cancellationToken);
                if (stream != null)
                {
                    var extension = Path.GetExtension(jobApplication.AdditionalDocumentUrl);
                    documents.Add(new DocumentDto
                    {
                        ContentType = FileUtils.GetContentType(extension),
                        FileName = $"{jobApplication.LastName}.{initial}.Supplement({projectLabel}){extension}",
                        Stream = stream
                    });
                }
            }

            return documents;
        }
    }
}
