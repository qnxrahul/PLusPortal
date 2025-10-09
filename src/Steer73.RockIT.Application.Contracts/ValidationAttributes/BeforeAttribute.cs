using System;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.ValidationAttributes;

public class BeforeAttribute : ValidationAttribute 
{
    public string OtherProperty { get; private set; }

    public string OtherPropertyDisplayName { get; set; }

    public BeforeAttribute(
        string otherProperty,
        string otherPropertyDisplayName)
    {
        OtherProperty = otherProperty;
        OtherPropertyDisplayName = otherPropertyDisplayName;
    }

    protected override ValidationResult IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(validationContext, nameof(validationContext));
       
        var otherProperty = validationContext.ObjectType.GetProperty(OtherProperty)!;
        if (otherProperty == null)
        {
            return new ValidationResult(
                string.Format("Could not find a property named '{0}'.", OtherProperty),
                [validationContext.MemberName!]);
        }

        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance)!;

        if (value is not null
            && otherValue is not null)    
        {
            if (value is DateOnly dateOnly
                && otherValue is DateOnly otherDateOnly
                && dateOnly < otherDateOnly)
            {
                return ValidationResult.Success!;
            }

            if (value is DateTime dateTime
                && otherValue is DateTime otherDateTime
                && dateTime < otherDateTime)
            {
                return ValidationResult.Success!;
            }
        }

        return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
    }

    public string GetErrorMessage(string displayName)
    {
        return $"The '{displayName}' value must be before '{OtherPropertyDisplayName}'";
    }
}




