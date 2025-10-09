using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.JobFormResponses
{
    public abstract class JobFormResponseManagerBase : DomainService
    {
        protected IJobFormResponseRepository _jobFormResponseRepository;

        public JobFormResponseManagerBase(IJobFormResponseRepository jobFormResponseRepository)
        {
            _jobFormResponseRepository = jobFormResponseRepository;
        }

        public virtual async Task<JobFormResponse> CreateAsync(
        Guid jobApplicationId, string formStructureJson, string formResponseJson)
        {
            Check.NotNullOrWhiteSpace(formStructureJson, nameof(formStructureJson));
            Check.NotNullOrWhiteSpace(formResponseJson, nameof(formResponseJson));

            var jobFormResponse = new JobFormResponse(
             GuidGenerator.Create(),
             jobApplicationId, formStructureJson, formResponseJson
             );

            return await _jobFormResponseRepository.InsertAsync(jobFormResponse);
        }

        public virtual async Task<JobFormResponse> UpdateAsync(
            Guid id,
            Guid jobApplicationId, string formStructureJson, string formResponseJson
        )
        {
            Check.NotNullOrWhiteSpace(formStructureJson, nameof(formStructureJson));
            Check.NotNullOrWhiteSpace(formResponseJson, nameof(formResponseJson));

            var jobFormResponse = await _jobFormResponseRepository.GetAsync(id);

            jobFormResponse.JobApplicationId = jobApplicationId;
            jobFormResponse.FormStructureJson = formStructureJson;
            jobFormResponse.FormResponseJson = formResponseJson;

            return await _jobFormResponseRepository.UpdateAsync(jobFormResponse);
        }

    }
}