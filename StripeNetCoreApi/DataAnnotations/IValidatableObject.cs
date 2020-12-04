using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DataAnnotations
{
    public interface IValidatableObject
    {
        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
