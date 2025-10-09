using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.ValidationAttributes;

public class RequiredEnumerableAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not null
            && value is IEnumerable list
            && list.GetEnumerator().MoveNext())
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult(GetErrorMessage(validationContext.DisplayName), [validationContext.DisplayName]);
    }

    public string GetErrorMessage(string displayName)
    {
        return $"The '{displayName}' field is required.";
    }
}



