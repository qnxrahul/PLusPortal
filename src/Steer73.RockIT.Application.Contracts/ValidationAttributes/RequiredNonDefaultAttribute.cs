using System;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.ValidationAttributes;

public class RequiredNonDefaultAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value,
        ValidationContext validationContext)
    {
        var type = value?.GetType();
        if (value is not null
            && !Equals(value, Activator.CreateInstance(Nullable.GetUnderlyingType(type!) ?? type!)))
        {
            return ValidationResult.Success!; 
        }
    
        return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
    }

    public string GetErrorMessage(string displayName)
    {
        return $"The '{displayName}' field is required.";
    }
}
