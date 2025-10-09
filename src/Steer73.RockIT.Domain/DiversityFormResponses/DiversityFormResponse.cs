using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Steer73.RockIT.DiversityFormResponses
{
    public abstract class DiversityFormResponseBase : FullAuditedEntity<Guid>
    {
        public virtual Guid JobApplicationId { get; set; }

        [NotNull]
        public virtual string FormStructureJson { get; set; }

        [NotNull]
        public virtual string FormResponseJson { get; set; }

        protected DiversityFormResponseBase()
        {

        }

        public DiversityFormResponseBase(Guid id, Guid jobApplicationId, string formStructureJson, string formResponseJson)
        {

            Id = id;
            Check.NotNull(formStructureJson, nameof(formStructureJson));
            Check.NotNull(formResponseJson, nameof(formResponseJson));
            JobApplicationId = jobApplicationId;
            FormStructureJson = formStructureJson;
            FormResponseJson = formResponseJson;
        }

    }
}