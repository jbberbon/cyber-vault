using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.Models.Validations;

public class StringLengthRangeAttribute : ValidationAttribute
{
    public int MinLength { get; }
    public int MaxLength { get; }

    public StringLengthRangeAttribute(int minLength = 3, int maxLength = 250)
    {
        MinLength = minLength;
        MaxLength = maxLength;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        var stringValue = value as string;
        
        if (stringValue!.Length < MinLength || stringValue!.Length > MaxLength)
        {
            return new ValidationResult($"The field {validationContext.DisplayName} must be between {MinLength} and {MaxLength} characters.");
        }
        return ValidationResult.Success!;
    }

}