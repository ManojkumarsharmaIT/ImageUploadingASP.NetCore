using System.ComponentModel.DataAnnotations;

namespace ImageUploadingASP.NetCore.Models
{
    public class RequiredForCreateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the object (model) being validated
            var model = (dynamic)validationContext.ObjectInstance;

            // Check if the model is being created (Id = 0) and the Photo is null
            if (model.Id == 0 && value == null)
            {
                return new ValidationResult(ErrorMessage ?? "The Photo field is required for new products.");
            }

            // Validation passed
            return ValidationResult.Success;
        }
    }
}
