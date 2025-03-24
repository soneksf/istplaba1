/*using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LibraryDomain.Models;         // For Position
using LibraryInfrastructure;        // For DblibraryContext

namespace LibraryDomain.Validation
{
    public class UniquePositionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Cast the object instance to Position.
            var position = (Position)validationContext.ObjectInstance;

            // Retrieve the DblibraryContext from the service provider.
            var contextObject = validationContext.GetService(typeof(DblibraryContext));
            if (contextObject is not DblibraryContext context)
            {
                throw new InvalidOperationException("DblibraryContext is not available in the service provider. Ensure that LibraryDomain references LibraryInfrastructure and that DblibraryContext is registered.");
            }

            // Check if any position already exists for the given employee (excluding the current record if editing).
            bool exists = context.Positions.Any(p =>
                p.EmployeeId == position.EmployeeId &&
                p.Id != position.Id);

            if (exists)
            {
                return new ValidationResult("Цей працівник вже має посаду. Неможливо створити більше однієї позиції для одного працівника.");
            }

            return ValidationResult.Success;
        }
    }
}*/
