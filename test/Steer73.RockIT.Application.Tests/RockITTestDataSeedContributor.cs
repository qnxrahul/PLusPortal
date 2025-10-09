using Steer73.RockIT.Companies;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Steer73.RockIT
{
    public class RockITTestDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IFormDefinitionRepository _formDefinitionRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPracticeGroupRepository _practiceGroupRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public RockITTestDataSeedContributor(
            IFormDefinitionRepository formDefinitionRepository,
            IVacancyRepository vacancyRepository,
            ICompanyRepository companyRepository,
            IPracticeGroupRepository practiceGroupRepository,
            IIdentityUserRepository identityUserRepository,
            IJobApplicationRepository jobApplicationRepository)
        {
            this._formDefinitionRepository = formDefinitionRepository;
            this._vacancyRepository = vacancyRepository;
            this._companyRepository = companyRepository;
            this._practiceGroupRepository = practiceGroupRepository;
            this._identityUserRepository = identityUserRepository;
            this._jobApplicationRepository = jobApplicationRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            await _identityUserRepository.InsertAsync(new IdentityUser(TestData.UserId, "A user", "randomuser@steer73.com"));

            await _companyRepository.InsertAsync(new Company(TestData.CompanyId));

            var practiceGroup = new PracticeGroup(TestData.PracticeGroupId, "Practice group test", true);    
            await _practiceGroupRepository.InsertAsync(practiceGroup);
            
            await _formDefinitionRepository.InsertAsync(
                new FormDefinition(TestData.JobFormDefinition1Id, "JF1", "Job form 1", FormType.VacancyDiversityType, TestData.CompanyId, TestData.TestJobFormStructure)
                );
            await _formDefinitionRepository.InsertAsync(
                new FormDefinition(TestData.DiversityFormDefinition1Id, "DF1", "Diversity form 1", FormType.DiversityType, TestData.CompanyId, TestData.TestDiversityFormStructure)
                );
            await _vacancyRepository.InsertAsync(
                new Vacancy(
                    TestData.Vacancy1Id,
                    TestData.CompanyId,
                    TestData.UserId,
                    [practiceGroup],
                    TestData.JobFormDefinition1Id,
                    TestData.DiversityFormDefinition1Id,
                    "Vacancy 1",
                    "VC1",
                    [Enums.Region.Europe],
                    [],
                    "This is the first test vacancy",
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    true,
                    false,
                    externalRefId: new Random().Next()));

            await _vacancyRepository.InsertAsync(
                new Vacancy(
                    TestData.Vacancy2Id,
                    TestData.CompanyId,
                    TestData.UserId,
                    [practiceGroup],
                    null,
                    null,
                    "Vacancy 1",
                    "VC1",
                    [Enums.Region.Europe],
                    [],
                    "This is the second test vacancy with no forms/diversity",
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    DateOnly.FromDateTime(DateTime.Now).AddDays(100),
                    false,
                    false,
                    externalRefId: new Random().Next())
                );
            await _jobApplicationRepository.InsertAsync(
                new JobApplication(
                    TestData.JobApplicationId,
                    TestData.Vacancy1Id,
                    "Test",
                    "Test",
                    "Test",
                    "test@steer73.com",
                    Enums.JobApplicationStatus.Pending,
                    Enums.SyncStatus.Pending,
                    Enums.SyncStatus.Pending,
                    Enums.SyncStatus.Pending,
                    "Some title",
                    "99889998989",
                    "76287362678",
                    "Janitor",
                    "TestCo",
                    "Worker",
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    externalRefId: new Random().Next()));
        }
    }
}
