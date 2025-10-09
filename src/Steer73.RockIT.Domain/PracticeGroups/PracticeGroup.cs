using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Steer73.RockIT.PracticeAreas;

using Volo.Abp;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

        public ICollection<PracticeArea> PracticeAreas { get; private set; }

        protected PracticeGroupBase()
        {

        }

        public PracticeGroupBase(Guid id, string name, bool isActive)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Check.Length(name, nameof(name), PracticeGroupConsts.NameMaxLength, 0);
            Name = name;
            IsActive = isActive;
            PracticeAreas = new Collection<PracticeArea>();
        }

    }
}