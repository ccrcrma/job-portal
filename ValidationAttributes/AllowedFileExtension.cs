using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace job_portal.ValidationAttributes
{
    public class AllowedFileExtension : ValidationAttribute
    {
        private readonly string[] _allowedExtension;

        public AllowedFileExtension(params string[] allowedExtension)
        {
            _allowedExtension = allowedExtension;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var formFile = (IFormFile)value;
            if (formFile != null)
            {
                var extension = System.IO.Path.GetExtension(formFile.FileName);
                if (!_allowedExtension.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage = $" extension {extension} not allowed only {string.Join(", ", _allowedExtension)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}