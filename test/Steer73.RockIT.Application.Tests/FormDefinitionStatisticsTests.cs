using Shouldly;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.Vacancies;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Xunit;

namespace Steer73.RockIT.Samples;

public abstract class FormDefinitionStatisticsTests<TStartupModule> : RockITApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IFormDefinitionsAppService _formDefinitionsAppService;
    private readonly VacancyManager _vacancyManager;

    protected FormDefinitionStatisticsTests()
    {
        _formDefinitionsAppService = GetRequiredService<IFormDefinitionsAppService>();
        _vacancyManager = GetRequiredService<VacancyManager>();
    }

    [Fact]
    public async Task Get_FormDefinitionStatistics()
    {
        //Arrange
        var testDate = new DateTime(2024, 06, 28);
        var companyId = TestData.CompanyId;
        var identityUser = TestData.UserId;
        //var practiceGroup = TestData.PracticeGroupId;
        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [], // No practice group is attached because the SQLIte provider sees exisitng practice groups fetched from the repository as new practice groups and tries to create them again leading to unique constraint violation.
            vacancyFormDefinitionId: null,
            diversityFormDefinitionId: null,
            title: "Vacancy11",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2023,09,09),
            closingDate: new DateOnly(2023, 10, 09),
            expiringDate: new DateOnly(2023, 11, 09),
            showDiversity: true,
            flagHideVacancy: true,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: null,
            diversityFormDefinitionId: TestData.DiversityFormDefinition1Id,
            title: "Vacancy12",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2024, 06, 10),
            closingDate: new DateOnly(2024, 08, 15),
            expiringDate: new DateOnly(2024, 08, 16),
            showDiversity: true,
            flagHideVacancy:false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: null,
            diversityFormDefinitionId: TestData.DiversityFormDefinition1Id,
            title: "Vacancy13",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2024, 06, 10),
            closingDate: new DateOnly(2024, 08, 15),
            expiringDate: new DateOnly(2024, 08, 16),
            showDiversity: true,
            flagHideVacancy:false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: TestData.DiversityFormDefinition1Id,
            diversityFormDefinitionId: TestData.DiversityFormDefinition1Id,
            title: "Vacancy14",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2023, 06, 10),
            closingDate: new DateOnly(2023, 08, 15),
            expiringDate: new DateOnly(2023, 08, 16),
            showDiversity: true,
            flagHideVacancy:false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: TestData.JobFormDefinition1Id,
            diversityFormDefinitionId: TestData.DiversityFormDefinition1Id,
            title: "Vacancy24",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2024, 06, 10),
            closingDate: new DateOnly(2024, 08, 15),
            expiringDate: new DateOnly(2024, 08, 16),
            showDiversity: true,
            flagHideVacancy:false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: TestData.JobFormDefinition1Id,
            diversityFormDefinitionId: TestData.JobFormDefinition1Id,
            title: "Vacancy23",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2023, 06, 10),
            closingDate: new DateOnly(2023, 08, 15),
            expiringDate: new DateOnly(2023, 08, 16),
            showDiversity: true,
            flagHideVacancy: false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: TestData.JobFormDefinition1Id,
            diversityFormDefinitionId: null,
            title: "Vacancy21",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2024, 06, 10),
            closingDate: new DateOnly(2024, 08, 15),
            expiringDate: new DateOnly(2024, 08, 16),
            showDiversity: true,
            flagHideVacancy: false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        await _vacancyManager.CreateAsync(
            companyId,
            identityUser,
            [],
            vacancyFormDefinitionId: TestData.JobFormDefinition1Id,
            diversityFormDefinitionId: null,
            title: "Vacancy22",
            reference: "ref",
            regions: [Enums.Region.Europe],
            [],
            description: "a lot of work",
            externalPostingDate: new DateOnly(2025, 06, 10),
            closingDate: new DateOnly(2025, 08, 15),
            expiringDate: new DateOnly(2025, 08, 16),
            showDiversity: true,
            flagHideVacancy: false,
            location: Guid.NewGuid().ToString(),
            salary: Guid.NewGuid().ToString());

        //Act
        var result = await _formDefinitionsAppService.GetFormDefinitionStatisticsDataAsync(testDate, [TestData.JobFormDefinition1Id, TestData.DiversityFormDefinition1Id]);

        //Assert
        result.Count.ShouldBe(2);

        var jobFormDefinitionResult = result.FirstOrDefault(x => x.FormDefinitionId == TestData.JobFormDefinition1Id);
        jobFormDefinitionResult.ShouldNotBeNull();
        jobFormDefinitionResult.VacanciesTotal.ShouldBe(5); // +1 vacancy is added by seed method
        jobFormDefinitionResult.PendingVacanciesCount.ShouldBe(2); // +1 vacancy is added by seed method and its always pending
        jobFormDefinitionResult.ClosedVacanciesCount.ShouldBe(1);
        jobFormDefinitionResult.ActiveVacanciesCount.ShouldBe(2);

        var diversityFormDefinitionResult = result.FirstOrDefault(x => x.FormDefinitionId == TestData.DiversityFormDefinition1Id);
        diversityFormDefinitionResult.ShouldNotBeNull();
        diversityFormDefinitionResult.VacanciesTotal.ShouldBe(5); // +1 vacancy is added by seed method
        diversityFormDefinitionResult.PendingVacanciesCount.ShouldBe(1); // +1 vacancy is added by seed method and its always pending
        diversityFormDefinitionResult.ClosedVacanciesCount.ShouldBe(1);
        diversityFormDefinitionResult.ActiveVacanciesCount.ShouldBe(3);
    }
}
