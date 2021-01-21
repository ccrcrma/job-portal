using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using job_portal.Util;
using Microsoft.AspNetCore.Http;

namespace job_portal.ValidationAttributes
{
    public class ValidateFileSignature : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var formFile = (FormFile)value;
            var fileExtension = Path.GetExtension(formFile.FileName);
            using (var reader = new BinaryReader(formFile.OpenReadStream()))
            {
                if (FileSignatures.signatures.TryGetValue(fileExtension, out List<byte[]> signatures))
                {
                    var maxHeaderSize = signatures.Max(s => s.Length);
                    var headerBytes = reader.ReadBytes(maxHeaderSize);
                    if (!signatures.Any(s => headerBytes.Take(s.Length).SequenceEqual(s)))
                    {
                        return new ValidationResult(ErrorMessage = $"File is not valid {fileExtension}");
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}