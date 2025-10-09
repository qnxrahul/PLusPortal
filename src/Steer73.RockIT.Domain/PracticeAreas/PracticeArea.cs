using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class PracticeAreaBase : FullAuditedEntity<Guid>
    {
        public virtual Guid PracticeGroupId { get; set; }

        [NotNull]
        public virtual string Name { get; set; }

        public virtual bool IsActive { get; set; }

        protected PracticeAreaBase()
        {

        }

        public PracticeAreaBase(Guid id, Guid practiceGroupId, string name, bool isActive)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Check.Length(name, nameof(name), PracticeAreaConsts.NameMaxLength, 0);
            PracticeGroupId = practiceGroupId;
            Name = name;
            IsActive = isActive;
        }

    }
}