using JetBrains.Annotations;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.RoleTypes
{
    public abstract class RoleTypeBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        protected RoleTypeBase()
        {

        }

        public RoleTypeBase(Guid id, string name, string description)
        {
            Id = id;
            Check.NotNull(name, nameof(name));
            Check.NotNull(description, nameof(description));

            Name = name;
            Description = description;
        }

    }
}
