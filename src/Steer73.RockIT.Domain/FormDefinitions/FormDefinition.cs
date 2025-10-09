using Steer73.RockIT.FormDefinitions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionBase : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string ReferenceId { get; set; }

        [NotNull]
        public virtual string Name { get; set; }

        [CanBeNull]
        public virtual string? FormDetails { get; set; }

        public virtual FormType FormType { get; set; }

        public Guid CompanyId { get; set; }

        protected FormDefinitionBase()
        {

        }

        public FormDefinitionBase(Guid id, string referenceId, string name, FormType formType, Guid companyId, string? formDetails = null)
        {

            Id = id;
            Check.NotNull(referenceId, nameof(referenceId));
            Check.Length(referenceId, nameof(referenceId), FormDefinitionConsts.ReferenceIdMaxLength, 0);
            Check.NotNull(name, nameof(name));
            Check.Length(name, nameof(name), FormDefinitionConsts.NameMaxLength, 0);
            ReferenceId = referenceId;
            Name = name;
            FormType = formType;
            FormDetails = formDetails;
            CompanyId = companyId;
        }

    }
}