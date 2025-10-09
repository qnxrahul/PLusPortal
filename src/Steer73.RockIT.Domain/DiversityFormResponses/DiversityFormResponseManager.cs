using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.DiversityFormResponses
{
    public abstract class DiversityFormResponseManagerBase : DomainService
    {
        protected IDiversityFormResponseRepository _diversityFormResponseRepository;

        public DiversityFormResponseManagerBase(IDiversityFormResponseRepository diversityFormResponseRepository)
        {
            _diversityFormResponseRepository = diversityFormResponseRepository;
        }

        public virtual async Task<DiversityFormResponse> CreateAsync(
        Guid jobApplicationId, string formStructureJson, string formResponseJson)
        {
            Check.NotNullOrWhiteSpace(formStructureJson, nameof(formStructureJson));
            Check.NotNullOrWhiteSpace(formResponseJson, nameof(formResponseJson));

            var diversityFormResponse = new DiversityFormResponse(
             GuidGenerator.Create(),
             jobApplicationId, formStructureJson, formResponseJson
             );

            return await _diversityFormResponseRepository.InsertAsync(diversityFormResponse);
        }

        public virtual async Task<DiversityFormResponse> UpdateAsync(
            Guid id,
            Guid jobApplicationId, string formStructureJson, string formResponseJson
        )
        {
            Check.NotNullOrWhiteSpace(formStructureJson, nameof(formStructureJson));
            Check.NotNullOrWhiteSpace(formResponseJson, nameof(formResponseJson));

            var diversityFormResponse = await _diversityFormResponseRepository.GetAsync(id);

            diversityFormResponse.JobApplicationId = jobApplicationId;
            diversityFormResponse.FormStructureJson = formStructureJson;
            diversityFormResponse.FormResponseJson = formResponseJson;

            return await _diversityFormResponseRepository.UpdateAsync(diversityFormResponse);
        }

    }
}