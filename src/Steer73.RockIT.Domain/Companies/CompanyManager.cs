using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyManagerBase : DomainService
    {
        protected ICompanyRepository _companyRepository;

        public CompanyManagerBase(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public virtual async Task<Company> CreateAsync(
        string? name = null, string? phone = null, string? address = null, string? postcode = null, string? primaryContact = null, string? logoUrl = null)
        {
            Check.Length(name, nameof(name), CompanyConsts.NameMaxLength);
            Check.Length(phone, nameof(phone), CompanyConsts.PhoneMaxLength);
            Check.Length(address, nameof(address), CompanyConsts.AddressMaxLength);
            Check.Length(postcode, nameof(postcode), CompanyConsts.PostcodeMaxLength);
            Check.Length(primaryContact, nameof(primaryContact), CompanyConsts.PrimaryContactMaxLength);
            Check.Length(logoUrl, nameof(logoUrl), CompanyConsts.LogoUrlMaxLength);

            var company = new Company(
             GuidGenerator.Create(),
             name, phone, address, postcode, primaryContact, logoUrl
             );

            return await _companyRepository.InsertAsync(company);
        }

        public virtual async Task<Company> UpdateAsync(
            Guid id,
            string? name = null, string? phone = null, string? address = null, string? postcode = null, string? primaryContact = null, string? logoUrl = null, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.Length(name, nameof(name), CompanyConsts.NameMaxLength);
            Check.Length(phone, nameof(phone), CompanyConsts.PhoneMaxLength);
            Check.Length(address, nameof(address), CompanyConsts.AddressMaxLength);
            Check.Length(postcode, nameof(postcode), CompanyConsts.PostcodeMaxLength);
            Check.Length(primaryContact, nameof(primaryContact), CompanyConsts.PrimaryContactMaxLength);
            Check.Length(logoUrl, nameof(logoUrl), CompanyConsts.LogoUrlMaxLength);

            var company = await _companyRepository.GetAsync(id);

            company.Name = name;
            company.Phone = phone;
            company.Address = address;
            company.Postcode = postcode;
            company.PrimaryContact = primaryContact;
            company.LogoUrl = logoUrl;

            company.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _companyRepository.UpdateAsync(company);
        }

    }
}