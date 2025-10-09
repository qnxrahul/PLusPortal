using Shouldly;
using Steer73.RockIT.DiversityDatas;
using Steer73.RockIT.DiversityFormResponses;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.JobFormResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Steer73.RockIT
{
    public abstract class JobApplicationAppServiceIntegrationTests<TStartupModule> : RockITApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IJobApplicationsAppService _jobApplicationAppService;
        private readonly IDiversityDatasAppService _diversityDataAppService;
        private readonly IJobFormResponsesAppService _jobFormResponseAppService;
        private readonly IDiversityFormResponsesAppService _diversityFormResponseAppService;

        protected JobApplicationAppServiceIntegrationTests()
        {
            _jobApplicationAppService = GetRequiredService<IJobApplicationsAppService>();
            _diversityDataAppService = GetRequiredService<IDiversityDatasAppService>();
            _jobFormResponseAppService = GetRequiredService<IJobFormResponsesAppService>();
            _diversityFormResponseAppService = GetRequiredService<IDiversityFormResponsesAppService>();
        }

        [Fact]
        public async Task TestApplyForJobComplete_HasChildData()
        {
            //Create a new full job application:

            var jobFormResponse = """"
                {
                    "question1": "Jon",
                    "question2": [
                        "Item 1",
                        "Item 2",
                        "Item 3"
                    ],
                    "question3": "Item 1",
                    "question4": "Item 1"
                }
                """";
            var newJob = new NewJobApplicationCompleteDto()
            {
                VacancyId = TestData.Vacancy1Id,
                FirstName = "Jon",
                LastName = "Bennett",
                Aka = "Jon",
                EmailAddress = "jon@steer73.com",
                Title = "Mr",
                PhoneNumber = "12345678",
                Landline = "54321678",
                CurrentRole = "Tables",
                CurrentCompany = "Self employed",
                CurrentPositionType = "Tables",
                Diversity_HappyToCompleteForm = Enums.YesNo.No,
                JobFormResponse = jobFormResponse,
                DiversityFormResponse = "A_DIVERSITY_FORM_RESPONSE",
                FileCv = new BlobDto
                {
                    Name = $"{Guid.NewGuid()}.pdf",
                    Content = "%PDF"u8.ToArray(),
                },
                FileCoverLetter = new BlobDto
                {
                    Name = $"{Guid.NewGuid()}.docx",
                    Content = [0x50, 0x4B, 0x03, 0x04],
                }
            };
            await _jobApplicationAppService.CreateNewJobApplicationCompleteAsync(newJob);

            //Check the job application exists:
            var jobApplications = await _jobApplicationAppService.GetListAsync(new GetJobApplicationsInput()
            {
                VacancyId = TestData.Vacancy1Id
            });
            jobApplications?.Items?.Count.ShouldBe(2);
            jobApplications?.Items.First()?.JobApplication.FirstName.ShouldBe("Jon");
            jobApplications?.Items.First()?.JobApplication.LastName.ShouldBe("Bennett");

            var applicationId = jobApplications?.Items.First()?.JobApplication.Id;

            //Check the diversity data was created:
            var diversityDatas = await _diversityDataAppService.GetListAsync(new GetDiversityDatasInput()
            {
                JobApplicationId = applicationId
            });
            diversityDatas?.Items?.Count.ShouldBe(1);
            diversityDatas?.Items?.First().HappyToCompleteForm.ShouldBe(Enums.YesNo.No);

            //Check the job form response was created:
            var jobFormResponses = await _jobFormResponseAppService.GetListAsync(new GetJobFormResponsesInput()
            {
                JobApplicationId = applicationId
            });
            jobFormResponses?.Items?.Count.ShouldBe(1);
            jobFormResponses?.Items?.First().FormStructureJson.ShouldBe(TestData.TestJobFormStructure);
            jobFormResponses?.Items?.First().FormResponseJson.ShouldBe(jobFormResponse);

            //Check the diversity form response was created:
            var diversityFormResponses = await _diversityFormResponseAppService.GetListAsync(new GetDiversityFormResponsesInput()
            {
                JobApplicationId = applicationId
            });
            diversityFormResponses?.Items?.Count.ShouldBe(1);
            diversityFormResponses?.Items?.First().FormStructureJson.ShouldBe(TestData.TestDiversityFormStructure);
            diversityFormResponses?.Items?.First().FormResponseJson.ShouldBe("A_DIVERSITY_FORM_RESPONSE");

        }

        [Fact]
        public async Task TestApplyForJobComplete_NoChildData()
        {
            //Create a new full job application:
            var newJob = new NewJobApplicationCompleteDto()
            {
                VacancyId = TestData.Vacancy2Id,
                FirstName = "Ron",
                LastName = "Bennett",
                Aka = "Jon",
                EmailAddress = "ron@steer73.com",
                Title = "Mr",
                PhoneNumber = "12345678",
                Landline = "54321678",
                CurrentRole = "Tables",
                CurrentCompany = "Self employed",
                CurrentPositionType = "Tables",
                FileCv = new BlobDto
                {
                    Name = $"{Guid.NewGuid()}.pdf",
                    Content = "%PDF"u8.ToArray(),
                },
                FileCoverLetter = new BlobDto
                {
                    Name = $"{Guid.NewGuid()}.docx",
                    Content = [0x50, 0x4B, 0x03, 0x04],
                }
            };
            await _jobApplicationAppService.CreateNewJobApplicationCompleteAsync(newJob);

            //Check the job application exists:
            var jobApplications = await _jobApplicationAppService.GetListAsync(new GetJobApplicationsInput()
            {
                VacancyId = TestData.Vacancy2Id
            });
            jobApplications?.Items?.Count.ShouldBe(1);

            var applicationId = jobApplications?.Items.First()?.JobApplication.Id;

            //Check the diversity data was NOT created:
            var diversityDatas = await _diversityDataAppService.GetListAsync(new GetDiversityDatasInput()
            {
                JobApplicationId = applicationId
            });
            diversityDatas?.Items?.Count.ShouldBe(0);

            //Check the job form response was NOT created:
            var jobFormResponses = await _jobFormResponseAppService.GetListAsync(new GetJobFormResponsesInput()
            {
                JobApplicationId = applicationId
            });
            jobFormResponses?.Items?.Count.ShouldBe(0);

            //Check the diversity form response was NOT created:
            var diversityFormResponses = await _diversityFormResponseAppService.GetListAsync(new GetDiversityFormResponsesInput()
            {
                JobApplicationId = applicationId
            });
            diversityFormResponses?.Items?.Count.ShouldBe(0);

        }
    }
}
