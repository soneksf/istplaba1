using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LibraryDomain.Validation
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = value as DateOnly?;
            PropertyInfo? property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                throw new ArgumentException($"Property {_comparisonProperty} not found.");
            }
            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateOnly?;

            
            if (currentValue.HasValue && comparisonValue.HasValue)
            {
                if (currentValue.Value < comparisonValue.Value)
                {
                    return new ValidationResult(ErrorMessage ??
                        $"{validationContext.DisplayName} cannot be earlier than {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
