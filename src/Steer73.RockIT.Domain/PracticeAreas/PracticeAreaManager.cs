using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class PracticeAreaManagerBase : DomainService
    {
        protected IPracticeAreaRepository _practiceAreaRepository;

        public PracticeAreaManagerBase(IPracticeAreaRepository practiceAreaRepository)
        {
            _practiceAreaRepository = practiceAreaRepository;
        }

        public virtual async Task<PracticeArea> CreateAsync(
        Guid practiceGroupId, string name, bool isActive)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), PracticeAreaConsts.NameMaxLength);

            var practiceArea = new PracticeArea(
             GuidGenerator.Create(),
             practiceGroupId, name, isActive
             );

            return await _practiceAreaRepository.InsertAsync(practiceArea);
        }

        public virtual async Task<PracticeArea> UpdateAsync(
            Guid id,
            Guid practiceGroupId, string name, bool isActive
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), PracticeAreaConsts.NameMaxLength);

            var practiceArea = await _practiceAreaRepository.GetAsync(id);

            practiceArea.PracticeGroupId = practiceGroupId;
            practiceArea.Name = name;
            practiceArea.IsActive = isActive;

            return await _practiceAreaRepository.UpdateAsync(practiceArea);
        }

    }
}