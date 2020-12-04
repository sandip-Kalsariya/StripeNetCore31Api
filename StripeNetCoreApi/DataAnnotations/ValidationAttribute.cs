using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DataAnnotations
{
    public abstract class ValidationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets an error message to associate with a validation control if validation fails.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the resource type to use for error-message lookup if validation fails.
        /// </summary>
        public Type ErrorMessageResourceType { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        public virtual bool RequiresValidationContext { get { return false; } }

        /// <summary>
        /// Checks whether the specified value is valid with respect to the current validation.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns></returns>
        public ValidationResult GetValidationResult(object value, ValidationContext validationContext)
        {
            var result = IsValid(value, validationContext);
            if (result == null)
                return ValidationResult.Success;
            return result;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool IsValid(object value)
        {
            return true;
        }

        protected virtual ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns></returns>
        public virtual string FormatErrorMessage(string name)
        {
            return string.Format(GetErrorString(), name);
        }

        protected string GetErrorString()
        {
            if (ErrorMessageResourceType == null)
                return ErrorMessage;
            var prop = ErrorMessageResourceType.GetRuntimeProperty(ErrorMessage);
            if (prop == null)
                throw new InvalidOperationException(
                    string.Format("The type {0} does not contain a property named {1}", ErrorMessageResourceType.FullName, ErrorMessage));
            var value = prop.GetValue(null);
            if (value == null) return null;
            return value.ToString();
        }
    }
}
