using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.ValidationAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value is BlobDto file)
            {
                if (file.Content.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName), [validationContext.MemberName!]);
                }
            }

            return ValidationResult.Success!;
        }

        public string GetErrorMessage(string displayName)
        {
            return $"Maximum allowed {displayName} file size is {_maxFileSize} bytes.";
        }
    }
}
