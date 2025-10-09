using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Steer73.RockIT.ValidationAttributes
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedFileExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value is not BlobDto file) { return ValidationResult.Success!; }

            var extension = Path.GetExtension(file.Name)?.ToLower();
            if (string.IsNullOrWhiteSpace(extension)
                || !_extensions.Contains(extension))
            {
                return new ValidationResult(
                    $"The {validationContext.DisplayName} file extension is not allowed.", 
                    [validationContext.MemberName!]);
            }

            var signatures = _fileSignatures[extension];
            if (signatures.Count > 0)
            {
                using var memoryStream = new MemoryStream(file.Content);
                using var reader = new BinaryReader(memoryStream);
                reader.BaseStream.Position = 0;
                var headerBytes = reader.ReadBytes(signatures.Max(n => n.Length));
                if (!signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature)))
                {
                    return new ValidationResult($"The {validationContext.DisplayName} file content is invalid.", [validationContext.MemberName!]);
                }
            }

            return ValidationResult.Success!;
        }

        /// <summary>
        /// More details here: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-7.0#file-signature-validation
        /// Signatures are available here: https://www.google.com/search?q=file+signatures+databases
        /// </summary>
        private static readonly Dictionary<string, List<byte[]>> _fileSignatures = new()
        {
            {
                ".docx",
                new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }
                }
            },

            {
                ".pdf",
                new List<byte[]>
                {
                    "%PDF"u8.ToArray()
                }
            }
        };
    }
}
