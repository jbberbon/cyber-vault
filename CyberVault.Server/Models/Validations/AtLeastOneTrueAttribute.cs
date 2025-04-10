using System;
using System.ComponentModel.DataAnnotations;

namespace CyberVault.Server.Models.Validations
{
    public class AtLeastOneTrueAttribute : ValidationAttribute
    {
        private readonly string _property1Name;
        private readonly string _property2Name;

        public AtLeastOneTrueAttribute(string property1Name, string property2Name)
        {
            _property1Name = property1Name;
            _property2Name = property2Name;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var property1 = context.ObjectType.GetProperty(_property1Name);
            var property2 = context.ObjectType.GetProperty(_property2Name);
            
            // Get property values
            var value1 = property1?.GetValue(context.ObjectInstance);
            var value2 = property2?.GetValue(context.ObjectInstance);
            
            // Check if either is true
            var isValid = value1 is bool and true || value2 is bool and true;
            
            if (isValid)
                return ValidationResult.Success;
                
            return new ValidationResult($"At least one of {_property1Name} or {_property2Name} must be true.");
        }
    }
}