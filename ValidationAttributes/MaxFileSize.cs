using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace job_portal.ValidationAttributes
{
    public class MaxFileSize : ValidationAttribute
    {
        private readonly int _sizeInKB;

        public MaxFileSize(int sizeInKB)
        {
            _sizeInKB = sizeInKB;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var formFile = (IFormFile)value;
            if (formFile?.Length > _sizeInKB*1000)
            {
                return new ValidationResult(ErrorMessage = $"File size can't be greater than {_sizeInKB}KB");
            }
            return ValidationResult.Success;

        }
    }
}