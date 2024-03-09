using System.ComponentModel.DataAnnotations;

namespace Services.Helpers;

public class ModelValidationHelper
{
    public static bool IsValid(object obj)
    {
        ValidationContext validationContext = new(obj);
        List<ValidationResult> validationResults = [];

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        if (!isValid) throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);

        return isValid;
    }
}