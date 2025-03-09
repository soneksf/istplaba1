using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Validation
{
    
    public class PastOrYesterdayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateOnly dateValue)
            {
                
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                
                if (dateValue < today)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage ?? "Date must be no later than yesterday.");
                }
            }

            return new ValidationResult("Invalid date type.");
        }
    }
}
