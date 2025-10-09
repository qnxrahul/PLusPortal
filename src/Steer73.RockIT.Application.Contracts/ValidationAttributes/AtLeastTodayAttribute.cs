using System;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.ValidationAttributes;

public class AtLeastTodayAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not null
            && ((value is DateOnly dateOnly && dateOnly >= DateOnly.FromDateTime(DateTime.UtcNow.Date))
            || (value is DateTime dateTime && dateTime >= DateTime.UtcNow.Date)))
        {
            return ValidationResult.Success!;
        }


        return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
    }

    public string GetErrorMessage(string displayName)
    {
        return $"The '{displayName}' value must not be before current day";
    }
}




