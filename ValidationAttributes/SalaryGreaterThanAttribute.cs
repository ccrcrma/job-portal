using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace job_portal.ValidationAttributes
{
    public class SalaryGreaterThanAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _comparisonProperty;

        public SalaryGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var error = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-error", error);
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var salaryMax = (decimal?)value;
            if (salaryMax != null)
            {
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                {
                    throw new ArgumentException("Property with this name not found");
                }

                var comparisonValue = (decimal?)property.GetValue(validationContext.ObjectInstance);
                if (comparisonValue != null && salaryMax <= comparisonValue)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}