using Shouldly;
using Steer73.RockIT.Enums;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using System;
using Xunit;

namespace Steer73.RockIT
{
    public class Vacancy_Tests
    {
        [Fact]
        public void Should_Return_The_VacancyStatus()
        {
            // Arrange

            DateTime testBeforePublishDate = new(2024, 08, 26);
            DateTime testExactPublishDate = new(2024, 09, 01);
            DateTime testActiveDate = new(2024, 09, 19);
            DateTime testExactClosingDate = new(2024, 10, 01);
            DateTime testAfterClosingDate = new(2024, 10, 03);
            DateOnly externalPostingDate = new(2024, 09, 01);
            DateOnly closingDate = new(2024, 10, 01);
            DateOnly expiringDate = new(2024, 09, 27);

            var vacancy = new Vacancy(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                [
                    new PracticeGroup(
                        Guid.NewGuid(), 
                        Guid.NewGuid().ToString(), 
                        true)
                ],
                Guid.NewGuid(),
                Guid.NewGuid(),
                "some title",
                "RR56789",
                [Region.Europe],
                [],
                "some description",
                externalPostingDate,
                closingDate,
                expiringDate,
                false,false);

            // Act

            var resultBeforePublishDate = vacancy.GetStatus(testBeforePublishDate);
            var resultExactPublishDate = vacancy.GetStatus(testExactPublishDate);
            var resultActiveDate = vacancy.GetStatus(testActiveDate);
            var resultExactClosingDate = vacancy.GetStatus(testExactClosingDate);
            var resultAfterClosingDate = vacancy.GetStatus(testAfterClosingDate);

            // Assert

            resultBeforePublishDate.ShouldBe(VacancyStatus.Pending);
            resultExactPublishDate.ShouldBe(VacancyStatus.Active);
            resultActiveDate.ShouldBe(VacancyStatus.Active);
            resultExactClosingDate.ShouldBe(VacancyStatus.Closed);
            resultAfterClosingDate.ShouldBe(VacancyStatus.Closed);
        }
    }
}
