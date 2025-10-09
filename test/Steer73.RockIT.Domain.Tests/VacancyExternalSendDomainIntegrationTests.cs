using EzekiaCRM;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Steer73.RockIT.AppFileDescriptors;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Domain.External;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Threading;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Steer73.RockIT
{
    public abstract class VacancyExternalSendDomainIntegrationTests<TStartupModule> : RockITDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        private const int Company1Id = 735568;
        private const int Company2Id = 735608;
        private const int Company3Id = 735603;
        private const int ExternalVacancyId = 5555;

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

        private readonly IClient _ezekiaClientFakeOkResult;
        private readonly IClient _ezekiaClientFakeErrorResult;
        

        protected VacancyExternalSendDomainIntegrationTests()
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
            _logger = Substitute.For<ILogger<ExternalCompanyService>>();
            _ezekiaClientFakeOkResult = Substitute.For<IClient>();
            _ezekiaClientFakeErrorResult = Substitute.For<IClient>();          
        }

        [Fact(Skip = "Test disabled temporarily")]
        public async System.Threading.Tasks.Task SendVacancyDataAsync_Error()
        {
            //arrange
            _ezekiaClientFakeErrorResult.ProjectsPutAsync(
                fields: Arg.Any<IEnumerable<field4>>(),
                id: Arg.Any<int>(),
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
                _vacancyContainer);
            var vacancyInit = await _vacancyRepository.GetAsync(Domain.Tests.TestData.Vacancy1Id);
            ApiException? exception = null;
          
            //action
            await WithUnitOfWorkAsync(async () =>
            {
                exception = await Should.ThrowAsync<ApiException>(async () =>

                    await externalCompanyService.SendVacancyDataAsync(
                            vacancyId: vacancyInit.Id,
                            cancellationToken: new CancellationToken())
                    );
            });

            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith("Response was null which was not expected");

            //assertion
            var vacancyUpdated = await _vacancyRepository.GetAsync(Domain.Tests.TestData.Vacancy1Id);
            vacancyUpdated.ShouldNotBeNull();
            vacancyUpdated.SyncStatus.ShouldBe(Enums.SyncStatus.Error);
            vacancyUpdated.SyncStatusUpdate.ShouldNotBeNull();

            var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(500);
        }

        [Fact(Skip = "Test disabled temporarily")]
        public async System.Threading.Tasks.Task SendVacancyDataAsync_Success()
        {
            //arrange
            var vacancyInit = await _vacancyRepository.GetAsync(Domain.Tests.TestData.Vacancy1Id);

            _ezekiaClientFakeOkResult.V3ProjectsGetAsync(
                id: Arg.Any<int>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response131
                {
                    Data = new default8
                    {
                        Id = vacancyInit.ExternalRefId!.Value,
                        ProjectId = ""
                    }
                }));

            _ezekiaClientFakeOkResult.ProjectsPutAsync(
                fields: null,
                id: Arg.Any<int>(),
                body: Arg.Any<object>(),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(System.Threading.Tasks.Task.FromResult(new Response57
                {
                    Data = new default4
                    {
                        Id = vacancyInit.ExternalRefId!.Value
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
                _vacancyContainer);


            //action
            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyService.SendVacancyDataAsync(
                    Domain.Tests.TestData.Vacancy1Id,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            var vacancyUpdated1 = await _vacancyRepository.GetAsync(Domain.Tests.TestData.Vacancy1Id);
            vacancyUpdated1.SyncStatus.ShouldBe(Enums.SyncStatus.Synced);
            vacancyUpdated1.SyncStatusUpdate.ShouldNotBeNull();
            vacancyUpdated1.ExternalRefId.ShouldBe(vacancyInit.ExternalRefId);
            
            var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(200);

            await _ezekiaClientFakeOkResult.Received(1).ProjectsPutAsync(null, Arg.Any<int>(), Arg.Any<object>(), Arg.Any<CancellationToken>());
        }
    }
}
