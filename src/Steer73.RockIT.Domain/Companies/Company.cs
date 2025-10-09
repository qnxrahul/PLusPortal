using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Steer73.RockIT.Companies
{
    public abstract class CompanyBase : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? Name { get; set; }

        [CanBeNull]
        public virtual string? Phone { get; set; }

        [CanBeNull]
        public virtual string? Address { get; set; }

        [CanBeNull]
        public virtual string? Postcode { get; set; }

        [CanBeNull]
        public virtual string? PrimaryContact { get; set; }

        [CanBeNull]
        public virtual string? LogoUrl { get; set; }

        protected CompanyBase()
        {

        }

        public CompanyBase(Guid id, string? name = null, string? phone = null, string? address = null, string? postcode = null, string? primaryContact = null, string? logoUrl = null)
        {

            Id = id;
            Check.Length(name, nameof(name), CompanyConsts.NameMaxLength, 0);
            Check.Length(phone, nameof(phone), CompanyConsts.PhoneMaxLength, 0);
            Check.Length(address, nameof(address), CompanyConsts.AddressMaxLength, 0);
            Check.Length(postcode, nameof(postcode), CompanyConsts.PostcodeMaxLength, 0);
            Check.Length(primaryContact, nameof(primaryContact), CompanyConsts.PrimaryContactMaxLength, 0);
            Check.Length(logoUrl, nameof(logoUrl), CompanyConsts.LogoUrlMaxLength, 0);
            Name = name;
            Phone = phone;
            Address = address;
            Postcode = postcode;
            PrimaryContact = primaryContact;
            LogoUrl = logoUrl;
        }

    }
}