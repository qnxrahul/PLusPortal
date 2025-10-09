using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupManagerBase : DomainService
    {
        protected IPracticeGroupRepository _practiceGroupRepository;

        public PracticeGroupManagerBase(IPracticeGroupRepository practiceGroupRepository)
        {
            _practiceGroupRepository = practiceGroupRepository;
        }

        public virtual async Task<PracticeGroup> CreateAsync(
        string name, bool isActive)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), PracticeGroupConsts.NameMaxLength);

            var practiceGroup = new PracticeGroup(
             GuidGenerator.Create(),
             name, isActive
             );

            return await _practiceGroupRepository.InsertAsync(practiceGroup);
        }

        public virtual async Task<PracticeGroup> UpdateAsync(
            Guid id,
            string name, bool isActive, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), PracticeGroupConsts.NameMaxLength);

            var practiceGroup = await _practiceGroupRepository.GetAsync(id);

            practiceGroup.Name = name;
            practiceGroup.IsActive = isActive;

            practiceGroup.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _practiceGroupRepository.UpdateAsync(practiceGroup);
        }

    }
}