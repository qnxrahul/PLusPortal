using EzekiaCRM;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Steer73.RockIT.AppFileDescriptors;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Domain.External;
using Steer73.RockIT.EzekiaSyncLogs;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Abp.Identity;
using Xunit;
using static Steer73.RockIT.Permissions.RockITSharedPermissions;

namespace Steer73.RockIT
{
    public abstract class JobApplicationExternalSendDomainIntegrationTests<TStartupModule> : RockITDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        private const int Company1Id = 735568;
        private const int Company2Id = 735608;
        private const int Company3Id = 735603;
        private const int ExternalJobApplicationId = 5555;

        private readonly ICompanyRepository _companyRepository;
        private readonly IAuditingManager _auditingManager;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILogger<ExternalCompanyService> _logger;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IRepository<AppFileDescriptor, Guid> _appFileDescriptorRepository;
        private readonly IBlobContainer<JobApplicantContainer> _jobApplicantContainer;
        private readonly IBlobContainer<VacancyFileContainer> _vacancyContainer;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly EzekiaSyncLogManager _ezekiaSyncLogManager;

        private readonly IClient _ezekiaClientFakeOkResult;
        private readonly IClient _ezekiaClientFakeErrorResult;
        

        protected JobApplicationExternalSendDomainIntegrationTests()
        {
            _companyRepository = GetRequiredService<ICompanyRepository>();
            _auditingManager = GetRequiredService<IAuditingManager>();
            _auditLogRepository = GetRequiredService<IAuditLogRepository>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
            _jobApplicationRepository = GetRequiredService<IJobApplicationRepository>();
            _vacancyRepository = GetRequiredService<IVacancyRepository>();
            _appFileDescriptorRepository = GetRequiredService<IRepository<AppFileDescriptor, Guid>>();
            _jobApplicantContainer = GetRequiredService<IBlobContainer<JobApplicantContainer>>();
            _vacancyContainer = GetRequiredService<IBlobContainer<VacancyFileContainer>>();
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
            _ezekiaSyncLogManager = GetRequiredService<EzekiaSyncLogManager>();
            _logger = Substitute.For<ILogger<ExternalCompanyService>>();
            _ezekiaClientFakeOkResult = Substitute.For<IClient>();
            _ezekiaClientFakeOkResult.CompaniesGetAsync(
                query: Arg.Any<string>(),
                filterOn: Arg.Any<IEnumerable<Anonymous>>(),
                sortBy: Arg.Any<SortBy?>(),
                fields: Arg.Any<IEnumerable<field>>(),
                exclude: Arg.Any<IEnumerable<field>>(),
                counts: Arg.Any<IEnumerable<CountableFields>>(),
                sortOrder: Arg.Any<SortOrder?>(),
                page: Arg.Any<int?>(),
                count: Arg.Any<int?>(),
                from: Arg.Any<int?>(),
                to: Arg.Any<int?>(),
                since: Arg.Any<string>(),
                before: Arg.Any<string>(),
                between: Arg.Any<string>(),
                view: Arg.Any<View?>(),
                withArchived: Arg.Any<bool?>(),
                archived: Arg.Any<bool?>(),
                fuzzy: Arg.Any<bool?>(),
                tags: Arg.Any<IEnumerable<int>>(),
                withPinned: Arg.Any<bool?>(),
                cancellationToken: Arg.Any<CancellationToken>())
               .Returns(System.Threading.Tasks.Task.FromResult(new Response2()
               {
                   Data =
                    [
                        new EzekiaCRM.Company
                         {
                             Id = Company1Id
                         }
                    ]

               }));

            _ezekiaClientFakeErrorResult = Substitute.For<IClient>();
            _ezekiaClientFakeErrorResult.CompaniesGetAsync(
                query: Arg.Any<string>(),
                filterOn: Arg.Any<IEnumerable<Anonymous>>(),
                sortBy: Arg.Any<SortBy?>(),
                fields: Arg.Any<IEnumerable<field>>(),
                exclude: Arg.Any<IEnumerable<field>>(),
                counts: Arg.Any<IEnumerable<CountableFields>>(),
                sortOrder: Arg.Any<SortOrder?>(),
                page: Arg.Any<int?>(),
                count: Arg.Any<int?>(),
                from: Arg.Any<int?>(),
                to: Arg.Any<int?>(),
                since: Arg.Any<string>(),
                before: Arg.Any<string>(),
                between: Arg.Any<string>(),
                view: Arg.Any<View?>(),
                withArchived: Arg.Any<bool?>(),
                archived: Arg.Any<bool?>(),
                fuzzy: Arg.Any<bool?>(),
                tags: Arg.Any<IEnumerable<int>>(),
                withPinned: Arg.Any<bool?>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response2()
                {
                     Data =
                     [
                         new EzekiaCRM.Company
                         {
                             Id = Company1Id
                         }
                     ]
                }));
            var identityUser = _identityUserRepository.GetAsync(Domain.Tests.TestData.UserId).GetAwaiter().GetResult();
            var ownerLookupResponse = new Response85
            {
                Data = new List<Researcher>
                {
                    new()
                    {
                        Id = 123456,
                        Email = identityUser.Email,
                        FirstName = identityUser.Name,
                        LastName = identityUser.Surname,
                        FullName = $"{identityUser.Name} {identityUser.Surname}"
                    }
                }
            };
            _ezekiaClientFakeOkResult.SearchUsersAsync(
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<int?>(),
                Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(ownerLookupResponse));
            _ezekiaClientFakeErrorResult.SearchUsersAsync(
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<int?>(),
                Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(ownerLookupResponse));
        }

        [Fact]
        public async System.Threading.Tasks.Task SendApplicantDataAsync_WhenCandidateDoesntExistOnEzekia_Error()
        {
            //arrange
            _ezekiaClientFakeErrorResult.V3PeopleGetAsync(
                query: Arg.Any<string>(),
                sortOrder: Arg.Any<SortOrder?>(),
                since: Arg.Any<string>(),
                before: Arg.Any<string>(),
                between: Arg.Any<string>(),
                view: Arg.Any<View?>(),
                fuzzy: Arg.Any<bool?>(),
                page: Arg.Any<int?>(),
                count: Arg.Any<int?>(),
                tags: Arg.Any<IEnumerable<int>>(),
                filterOn: Arg.Any<IEnumerable<Anonymous6>>(),
                sortBy: Arg.Any<SortBy6?>(),
                isCandidate: Arg.Any<bool?>(),
                onCompanies: Arg.Any<bool?>(), // Should this be true?
                fields: Arg.Any<IEnumerable<field10>>(),
                exclude: Arg.Any<IEnumerable<field10>>(),
                counts: Arg.Any<IEnumerable<countableFields7>>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response123()
                {
                    Data =
                    [

                    ]
                }));

            _ezekiaClientFakeErrorResult.V3PeoplePostAsync(
                fields: Arg.Any<IEnumerable<field10>>(),
                body: Arg.Any<object>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Throws(new ApiException("Response was null which was not expected", 422, "some response", null, null));

            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeErrorResult,
                _auditingManager,
                _jobApplicationRepository,
                _vacancyRepository,
                _unitOfWorkManager,
                _logger,
                _appFileDescriptorRepository,
                _jobApplicantContainer,
                _vacancyContainer,
                _identityUserRepository,
                _ezekiaSyncLogManager);
            var jobApplicationInit = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);
            ApiException? exception = null;

            //action
            await WithUnitOfWorkAsync(async () =>
            {
                exception = await Should.ThrowAsync<ApiException>(async () =>

                    await externalCompanyService.SendApplicantDataAsync(
                        jobApplicationInit.Id,
                        cancellationToken: new CancellationToken()));
            });

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("Response was null which was not expected");

            //assertion
            var jobApplicationUpdated = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);
            jobApplicationUpdated.ShouldNotBeNull();
            jobApplicationUpdated.SyncStatus.ShouldBe(Enums.SyncStatus.Error);
            jobApplicationUpdated.SyncStatusUpdate.ShouldNotBeNull();
            jobApplicationUpdated.ExternalRefId.ShouldBeNull();

            var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(500);
        }

        [Fact]
        public async System.Threading.Tasks.Task SendApplicantDataAsync_WhenCandidateDoesntExistOnEzekia_Success()
        {
            //arrange
            _ezekiaClientFakeOkResult.V3PeopleGetAsync(
                query: Arg.Any<string>(),
                sortOrder: Arg.Any<SortOrder?>(),
                since: Arg.Any<string>(),
                before: Arg.Any<string>(),
                between: Arg.Any<string>(),
                view: Arg.Any<View?>(),
                fuzzy: Arg.Any<bool?>(),
                page: Arg.Any<int?>(),
                count: Arg.Any<int?>(),
                tags: Arg.Any<IEnumerable<int>>(),
                filterOn: Arg.Any<IEnumerable<Anonymous6>>(),
                sortBy: Arg.Any<SortBy6?>(),
                isCandidate: Arg.Any<bool?>(),
                onCompanies: Arg.Any<bool?>(), // Should this be true?
                fields: Arg.Any<IEnumerable<field10>>(),
                exclude: Arg.Any<IEnumerable<field10>>(),
                counts: Arg.Any<IEnumerable<countableFields7>>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response123()
                {
                    Data =
                    [

                    ]
                }));

            _ezekiaClientFakeOkResult.V3PeoplePostAsync(
                fields: null,
                body: Arg.Any<object>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response124
                {
                    Data = new person2
                    {
                        Id = ExternalJobApplicationId
                    }
                }));

            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeOkResult,
                _auditingManager,
                _jobApplicationRepository,
                _vacancyRepository,
                _unitOfWorkManager,
                _logger,
                _appFileDescriptorRepository,
                _jobApplicantContainer,
                _vacancyContainer,
                _identityUserRepository,
                _ezekiaSyncLogManager);
            var jobApplicationInit = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);

            //action
            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyService.SendApplicantDataAsync(
                    jobApplicationInit.Id,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            var jobApplicationUpdated = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);
            jobApplicationUpdated.SyncStatus.ShouldBe(Enums.SyncStatus.Synced);
            jobApplicationUpdated.SyncStatusUpdate.ShouldNotBeNull();
			jobApplicationUpdated.ExternalRefId.ShouldBe(ExternalJobApplicationId);

			var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(200);
        }

        [Fact]
        public async System.Threading.Tasks.Task SendApplicantDataAsync_WhenCandidateExistsOnEzekia_Success()
        {
            // Arrange
            _ezekiaClientFakeOkResult.V3PeopleGetAsync(
                query: Arg.Any<string>(),
                sortOrder: Arg.Any<SortOrder?>(),
                since: Arg.Any<string>(),
                before: Arg.Any<string>(),
                between: Arg.Any<string>(),
                view: Arg.Any<View?>(),
                fuzzy: Arg.Any<bool?>(),
                page: Arg.Any<int?>(),
                count: Arg.Any<int?>(),
                tags: Arg.Any<IEnumerable<int>>(),
                filterOn: Arg.Any<IEnumerable<Anonymous6>>(),
                sortBy: Arg.Any<SortBy6?>(),
                isCandidate: Arg.Any<bool?>(),
                onCompanies: Arg.Any<bool?>(), // Should this be true?
                fields: Arg.Any<IEnumerable<field10>>(),
                exclude: Arg.Any<IEnumerable<field10>>(),
                counts: Arg.Any<IEnumerable<countableFields7>>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response123()
                {
                    Data =
                    [
                        new person2() 
                        {
                            Id =  ExternalJobApplicationId
                        }
                    ]
                }));

            _ezekiaClientFakeOkResult.V3PeoplePutAsync(
                fields: null,
                id: ExternalJobApplicationId,
                body: Arg.Any<object>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response126
                {
                    Data = new person2
                    {
                        Id = ExternalJobApplicationId
                    }
                }));

            
            _ezekiaClientFakeOkResult.V3ProjectsCandidatesPostAsync(
                Arg.Any<candidatesUpdateRequest2>(),
                Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new candidatesUpdateResponse2
                {
                    // Add any properties here if needed
                }));
            
            _ezekiaClientFakeOkResult.V2PeoplePositionsPostAsync(
                personId: ExternalJobApplicationId,
                body: Arg.Any<storeRequest11>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response97
                {
                }));

            _ezekiaClientFakeOkResult.PeoplePhonesPostAsync(
                personId: ExternalJobApplicationId,
                body: Arg.Any<phones2>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new PhonePostResponse
                {
                }));

            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeOkResult,
                _auditingManager,
                _jobApplicationRepository,
                _vacancyRepository,
                _unitOfWorkManager,
                _logger,
                _appFileDescriptorRepository,
                _jobApplicantContainer,
                _vacancyContainer,
                _identityUserRepository,
                _ezekiaSyncLogManager);
            var jobApplicationInit = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);

            //action
            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyService.SendApplicantDataAsync(
                    jobApplicationInit.Id,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            var jobApplicationUpdated = await _jobApplicationRepository.GetAsync(Domain.Tests.TestData.JobApplicationId);
            jobApplicationUpdated.SyncStatus.ShouldBe(Enums.SyncStatus.Synced);
            jobApplicationUpdated.SyncStatusUpdate.ShouldNotBeNull();
            jobApplicationUpdated.ExternalRefId.ShouldBe(ExternalJobApplicationId);

            var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(200);

            await _ezekiaClientFakeOkResult.Received(1).V3PeoplePutAsync(
                fields: null,
                id: ExternalJobApplicationId,
                body: Arg.Any<object>(),
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFakeOkResult.Received(1).V3ProjectsCandidatesPostAsync(
    Arg.Any<candidatesUpdateRequest2>(),
    Arg.Any<CancellationToken>());


            await _ezekiaClientFakeOkResult.Received(1).V2PeoplePositionsPostAsync(
                personId: ExternalJobApplicationId,
                body: Arg.Any<storeRequest11>(),
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFakeOkResult.Received(2).PeoplePhonesPostAsync(
                personId: ExternalJobApplicationId,
                body: Arg.Any<phones2>(),
                cancellationToken: Arg.Any<CancellationToken>());
        }
    }
}
