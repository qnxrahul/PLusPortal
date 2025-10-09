using EzekiaCRM;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Steer73.RockIT.AppFileDescriptors;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Domain.External;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class CompanyListUpdateDomainIntegrationTests<TStartupModule> : RockITDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
    {
        private const int Company1Id = 735568;
        private const int Company2Id = 735608;
        private const int Company3Id = 735603;

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

        private readonly IClient _ezekiaClientFakeAllResults;
        private readonly IClient _ezekiaClientFake1PerPageResult;
        private readonly IClient _ezekiaClientFakeAllResultsUpdated;

        protected CompanyListUpdateDomainIntegrationTests()
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
            _ezekiaClientFakeAllResults = Substitute.For<IClient>();
            _ezekiaClientFakeAllResults.CompaniesGetAsync(
                query: Arg.Any<string>(),
                filterOn: Arg.Any<IEnumerable<Anonymous>>(),
                sortBy: SortBy.Id,
                fields: Arg.Any<IEnumerable<field>>(),
                exclude: Arg.Any<IEnumerable<field>>(),
                counts: Arg.Any<IEnumerable<CountableFields>>(),
                sortOrder: Arg.Any<SortOrder>(),
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
                // Get id of first company
                .Returns(x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data = new List<EzekiaCRM.Company>
                    {
                        new()
                        {
                            Id = Company1Id
                        }
                    }
                }),
                // Get id of last company
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data = new List<EzekiaCRM.Company>
                    {
                        new()
                        {
                            Id = Company2Id
                        }
                    }
                }),
                // Last page
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company1Id,
                            DataType = "company",
                            Name = "S73QA",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "client",
                            Aliases = null,
                            Division = "Subsidary",
                            Description = null,
                            Industries = null,
                            Address = new Address
                            {
                                Id = 2032857,
                                City = "First Floor",
                                State = "London",
                                Country = "UK",
                                Postcode = "w1w57t",
                                Full = null,
                                IsDefault = true,
                                Line1 = "S73 85 Great"
                            },
                            Speciality = null,
                            Size = "40",
                            Image = new Image4
                            {
                                Url = "https://steer73.com/rock_logo",
                                Color = null
                            },
                            Phone = "7092274482",
                            Email = "sajith@steer73.com",
                            CreatedAt = "2024-09-05T17:09:02.000000Z",
                            UpdatedAt = "2024-09-06T08:39:29.000000Z"
                        },
                        new()
                        {
                            Id = Company2Id,
                            DataType = "company",
                            Name = "S73 DigitalMark",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "company",
                            Aliases = new [] { "S73DM" },
                            Division = "The subsidiary of the company S73",
                            Description = "The description of the company",
                            Industries = Array.Empty<Industry>(),
                            Address = null,
                            Speciality = "The Digital Marketing company",
                            Size = "50",
                            Image = new Image4
                            {
                                Url = string.Empty,
                                Color = null
                            },
                            Phone = "+123 456 789",
                            Email = "sajith+04@steer73.com",
                            CreatedAt = "2024-09-16T13:31:19.000000Z",
                            UpdatedAt = "2024-09-16T13:31:20.000000Z"
                        }
                    ]
                }));

            _ezekiaClientFake1PerPageResult = Substitute.For<IClient>();
            _ezekiaClientFake1PerPageResult.CompaniesGetAsync(
                query: Arg.Any<string>(),
                filterOn: Arg.Any<IEnumerable<Anonymous>>(),
                sortBy: SortBy.Id,
                fields: Arg.Any<IEnumerable<field>>(),
                exclude: Arg.Any<IEnumerable<field>>(),
                counts: Arg.Any<IEnumerable<CountableFields>>(),
                sortOrder: Arg.Any<SortOrder>(),
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
                // Get id of first company
                .Returns(x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company1Id
                        }
                    ]
                }),
                // Get id of last company
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company2Id
                        }
                    ]
                }),
                // First page
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company1Id,
                            DataType = "company",
                            Name = "S73QA",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "client",
                            Aliases = null,
                            Division = "Subsidary",
                            Description = null,
                            Industries = null,
                            Address = new Address
                            {
                                Id = 2032857,
                                City = "First Floor",
                                State = "London",
                                Country = "UK",
                                Postcode = "w1w57t",
                                Full = null,
                                IsDefault = true,
                                Line1 = "S73 85 Great"
                            },
                            Speciality = null,
                            Size = "40",
                            Image = new Image4
                            {
                                Url = string.Empty,
                                Color = null
                            },
                            Phone = "7092274482",
                            Email = "sajith@steer73.com",
                            CreatedAt = "2024-09-05T17:09:02.000000Z",
                            UpdatedAt = "2024-09-06T08:39:29.000000Z"
                        }
                    ]
                }),
                // Last page
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company2Id,
                            DataType = "company",
                            Name = "S73 DigitalMark",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "company",
                            Aliases = new [] { "S73DM" },
                            Division = "The subsidiary of the company S73",
                            Description = "The description of the company",
                            Industries = Array.Empty<Industry>(),
                            Address = null,
                            Speciality = "The Digital Marketing company",
                            Size = "50",
                            Image = new Image4
                            {
                                Url = string.Empty,
                                Color = null
                            },
                            Phone = "+123 456 789",
                            Email = "sajith+04@steer73.com",
                            CreatedAt = "2024-09-16T13:31:19.000000Z",
                            UpdatedAt = "2024-09-16T13:31:20.000000Z"
                        }
                    ]
                 }));
           
            _ezekiaClientFakeAllResultsUpdated = Substitute.For<IClient>();
            _ezekiaClientFakeAllResultsUpdated.CompaniesGetAsync(
                query: Arg.Any<string>(),
                filterOn: Arg.Any<IEnumerable<Anonymous>>(),
                sortBy: SortBy.Id,
                fields: Arg.Any<IEnumerable<field>>(),
                exclude: Arg.Any<IEnumerable<field>>(),
                counts: Arg.Any<IEnumerable<CountableFields>>(),
                sortOrder: Arg.Any<SortOrder>(),
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
                // Get Id of first company
                .Returns(x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data = 
                    [
                         new()
                         {
                            Id = Company1Id
                         }
                    ]
                }),
                // Get id of last company
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company2Id
                        }
                    ]
                }),
                // Last page
                x => System.Threading.Tasks.Task.FromResult(new Response2
                {
                    Data =
                    [
                        new()
                        {
                            Id = Company1Id,
                            DataType = "company",
                            Name = "Updated S73QA",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "client",
                            Aliases = null,
                            Division = "Subsidary",
                            Description = null,
                            Industries = null,
                            Address = new Address
                            {
                                Id = 2032857,
                                City = "First Floor",
                                State = "London",
                                Country = "UK",
                                Postcode = "Updated w1w57t",
                                Full = null,
                                IsDefault = true,
                                Line1 = "S73 85 Great"
                            },
                            Speciality = null,
                            Size = "40",
                            Image = new Image4
                            {
                                Url = "Updated",
                                Color = null
                            },
                            Phone = "Updated 7092274482",
                            Email = "Updated sajith@steer73.com",
                            CreatedAt = "2024-09-05T17:09:02.000000Z",
                            UpdatedAt = "2024-09-06T08:39:29.000000Z"
                        },
                        new()
                        {
                            Id = 735603,
                            DataType = "company",
                            Name = "Created After Update",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "company",
                            Aliases = new [] { "S73DM" },
                            Division = "The subsidiary of the company S73",
                            Description = "The description of the company",
                            Industries = Array.Empty<Industry>(),
                            Address = null,
                            Speciality = "The Digital Marketing company",
                            Size = "50",
                            Image = new Image4
                            {
                                Url = "Created",
                                Color = null
                            },
                            Phone = "Created +123 456 789",
                            Email = "Created sajith+04@steer73.com",
                            CreatedAt = "2024-09-16T13:31:19.000000Z",
                            UpdatedAt = "2024-09-16T13:31:20.000000Z"
                        },
                        new()
                        {
                            Id = Company2Id,
                            DataType = "company",
                            Name = "Updated S73 DigitalMark",
                            Owner = new Researcher
                            {
                                Id = 1551265,
                                Email = "sajith@steer73.com",
                                FirstName = "Sajith",
                                LastName = "R",
                                FullName = "Sajith R",
                                Phone = "",
                                Location = null,
                                ProfilePicture = null
                            },
                            Label = "company",
                            Aliases = new [] { "S73DM" },
                            Division = "The subsidiary of the company S73",
                            Description = "The description of the company",
                            Industries = Array.Empty<Industry>(),
                            Address = null,
                            Speciality = "The Digital Marketing company",
                            Size = "50",
                            Image = new Image4
                            {
                                Url = "Updated",
                                Color = null
                            },
                            Phone = "Updated +123 456 789",
                            Email = "Updated sajith+04@steer73.com",
                            CreatedAt = "2024-09-16T13:31:19.000000Z",
                            UpdatedAt = "2024-09-16T13:31:20.000000Z"
                        }
                    ]
                }));
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCompaniesAsync_Create_Success()
        {
            //arrange
            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeAllResults,
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
                await _companyRepository.DeleteAllAsync();
                await externalCompanyService.UpdateCompaniesAsync(
                    Company2Id - Company1Id,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            var companies = await _companyRepository.GetListAsync();
            companies.Where(x => x.Name != null).Count().ShouldBe(2);

            var firstCompany = companies.FirstOrDefault(x => x.ExternalRefId == Company1Id);
            firstCompany.Name.ShouldBe("S73QA");
            firstCompany.Phone.ShouldBe("7092274482");
            firstCompany.Postcode.ShouldBe("w1w57t");
            firstCompany.PrimaryContact.ShouldBe("sajith@steer73.com");
            firstCompany.LogoUrl.ShouldBe("https://steer73.com/rock_logo");
            firstCompany.Address.ShouldBe("S73 85 Great First Floor London UK w1w57t");

            var secondCompany = companies.FirstOrDefault(x => x.ExternalRefId == Company2Id);
            secondCompany.Name.ShouldBe("S73 DigitalMark");
            secondCompany.Phone.ShouldBe("+123 456 789");
            secondCompany.Postcode.ShouldBeNull();
            secondCompany.PrimaryContact.ShouldBe("sajith+04@steer73.com");
            secondCompany.LogoUrl.ShouldBe(string.Empty);
            secondCompany.Address.ShouldBeNullOrEmpty();

            var logEntry = await _auditLogRepository.GetAsync(x => x.UserName == CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.ShouldNotBeNull();
            logEntry.ClientName.ShouldBe(CompanyConsts.BackgroundWorkerLogUserName);
            logEntry.HttpStatusCode.ShouldBe(200);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCompaniesAsync_Update_Success()
        {
            //arrange
            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeAllResults,
                _auditingManager,
                _jobApplicationRepository,
                _vacancyRepository,
                _unitOfWorkManager,
                _logger, 
                _appFileDescriptorRepository,
                _jobApplicantContainer,
                _vacancyContainer);
            var externalCompanyServiceUpdatedData = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFakeAllResultsUpdated,
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
                await _companyRepository.DeleteAllAsync();

                await externalCompanyService.UpdateCompaniesAsync(
                    Company2Id - Company1Id,
                    cancellationToken: new CancellationToken());
            });

            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyServiceUpdatedData.UpdateCompaniesAsync(
                    Company2Id - Company1Id,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            var companies = await _companyRepository.GetListAsync();
            companies.Count.ShouldBe(3);

            var firstCompany = companies.FirstOrDefault(x => x.ExternalRefId == Company1Id);
            firstCompany.Name.ShouldBe("Updated S73QA");
            firstCompany.Phone.ShouldBe("Updated 7092274482");
            firstCompany.Postcode.ShouldBe("Updated w1w57t");
            firstCompany.PrimaryContact.ShouldBe("Updated sajith@steer73.com");
            firstCompany.LogoUrl.ShouldBe("Updated");
            firstCompany.Address.ShouldBe("S73 85 Great First Floor London UK Updated w1w57t");

            var secondCompany = companies.FirstOrDefault(x => x.ExternalRefId == Company2Id);
            secondCompany.Name.ShouldBe("Updated S73 DigitalMark");
            secondCompany.Phone.ShouldBe("Updated +123 456 789");
            secondCompany.Postcode.ShouldBeNull();
            secondCompany.PrimaryContact.ShouldBe("Updated sajith+04@steer73.com");
            secondCompany.LogoUrl.ShouldBe("Updated");
            secondCompany.Address.ShouldBeNullOrEmpty();

            var thirdCompany = companies.FirstOrDefault(x => x.ExternalRefId == Company3Id);
            thirdCompany.Name.ShouldBe("Created After Update");
            thirdCompany.Phone.ShouldBe("Created +123 456 789");
            thirdCompany.Postcode.ShouldBeNull();
            thirdCompany.PrimaryContact.ShouldBe("Created sajith+04@steer73.com");
            thirdCompany.LogoUrl.ShouldBe("Created");
            thirdCompany.Address.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCompaniesAsync_Create_OnePage_Success()
        {
            var externalCompanyService = new ExternalCompanyService(
               _companyRepository,
               _ezekiaClientFakeAllResults,
               _auditingManager,
               _jobApplicationRepository,
               _vacancyRepository,
               _unitOfWorkManager,
               _logger,
               _appFileDescriptorRepository,
               _jobApplicantContainer,
               _vacancyContainer);

            var resultsPerPage = Company2Id - Company1Id;

            //action
            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyService.UpdateCompaniesAsync(
                    resultsPerPage,
                    cancellationToken: new CancellationToken());
            });

            //assertion
            await _ezekiaClientFakeAllResults.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Asc),
                page: null,
                count: Arg.Is(1),
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
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFakeAllResults.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Desc),
                page: null,
                count: Arg.Is(1),
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
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFakeAllResults.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Asc),
                page: null,
                count: Arg.Is(resultsPerPage),
                from: Arg.Is(Company1Id - 1), // value specified is non-inclusive
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
                cancellationToken: Arg.Any<CancellationToken>());
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCompaniesAsync_Create_MultiplePages_Success()
        {
            //arrange
            var externalCompanyService = new ExternalCompanyService(
                _companyRepository,
                _ezekiaClientFake1PerPageResult,
                _auditingManager,
                _jobApplicationRepository,
                _vacancyRepository,
                _unitOfWorkManager,
                _logger, 
                _appFileDescriptorRepository, 
                _jobApplicantContainer,
                _vacancyContainer);
            var numberOfPages = 2;
            var resultsPerPage = (Company2Id - Company1Id)/numberOfPages;

            //action
            await WithUnitOfWorkAsync(async () =>
            {
                await externalCompanyService.UpdateCompaniesAsync(resultsPerPage, cancellationToken: new CancellationToken());
            });

            //assertion
            await _ezekiaClientFake1PerPageResult.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Asc),
                page: null,
                count: Arg.Is(1),
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
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFake1PerPageResult.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Desc),
                page: null,
                count: Arg.Is(1),
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
                cancellationToken: Arg.Any<CancellationToken>());

            await _ezekiaClientFake1PerPageResult.Received(1).CompaniesGetAsync(
                query: null,
                filterOn: null,
                sortBy: Arg.Is(SortBy.Id),
                fields: null,
                exclude: null,
                counts: null,
                sortOrder: Arg.Is(SortOrder.Asc),
                page: null,
                count: Arg.Is(resultsPerPage),
                from: Arg.Is(Company1Id - 1),
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
                cancellationToken: Arg.Any<CancellationToken>());
        }
    }
}
