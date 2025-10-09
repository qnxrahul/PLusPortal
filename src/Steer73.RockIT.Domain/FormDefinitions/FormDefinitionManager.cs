using Steer73.RockIT.FormDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionManagerBase : DomainService
    {
        protected IFormDefinitionRepository _formDefinitionRepository;

        public FormDefinitionManagerBase(IFormDefinitionRepository formDefinitionRepository)
        {
            _formDefinitionRepository = formDefinitionRepository;
        }

        public virtual async Task<FormDefinition> CreateAsync(
        string referenceId, string name, FormType formType, Guid companyId, string? formDetails = null)
        {
            Check.NotNullOrWhiteSpace(referenceId, nameof(referenceId));
            Check.Length(referenceId, nameof(referenceId), FormDefinitionConsts.ReferenceIdMaxLength);
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), FormDefinitionConsts.NameMaxLength);
            Check.NotNull(formType, nameof(formType));

            var formDefinition = new FormDefinition(
             GuidGenerator.Create(),
             referenceId, name, formType, companyId, formDetails
             );

            return await _formDefinitionRepository.InsertAsync(formDefinition);
        }

        public virtual async Task<FormDefinition> UpdateAsync(
            Guid id,
            string referenceId, string name, FormType formType, Guid companyId, string? formDetails = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(referenceId, nameof(referenceId));
            Check.Length(referenceId, nameof(referenceId), FormDefinitionConsts.ReferenceIdMaxLength);
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.Length(name, nameof(name), FormDefinitionConsts.NameMaxLength);
            Check.NotNull(formType, nameof(formType));

            var formDefinition = await _formDefinitionRepository.GetAsync(id);

            formDefinition.ReferenceId = referenceId;
            formDefinition.Name = name;
            formDefinition.FormType = formType;
            formDefinition.FormDetails = formDetails;
            formDefinition.CompanyId = companyId;

            formDefinition.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _formDefinitionRepository.UpdateAsync(formDefinition, autoSave: true);
        }

    }
}