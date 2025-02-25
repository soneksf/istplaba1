using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Validation
{
    // This attribute checks that a DateOnly value is <= yesterday.
    public class PastOrYesterdayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // If it's null, let the [Required] handle it. 
                // Or you can treat null as invalid here too.
                return ValidationResult.Success;
            }

            if (value is DateOnly dateValue)
            {
                // DateOnly for "today"
                DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                // If dateValue is strictly less than "today", it's okay
                // i.e. it can be equal to "yesterday" or earlier
                if (dateValue < today)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    // dateValue >= today => not allowed
                    return new ValidationResult(ErrorMessage ?? "Date must be no later than yesterday.");
                }
            }

            // If somehow the property isn't a DateOnly, treat that as invalid
            return new ValidationResult("Invalid date type.");
        }
    }
}
