using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DataAnnotations
{
    public class ValidationResult
    {
        /// <summary>
        /// Represents the success of the validation (true if validation was successful; otherwise, false).
        /// </summary>
        public static readonly ValidationResult Success = new ValidationResult("");

        /// <summary>
        /// Gets the error message for the validation.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The collection of member names that indicate which fields have validation errors.
        /// </summary>
        public IEnumerable<string> MemberNames { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class by using an error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public ValidationResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class by using a ValidationResult object.
        /// </summary>
        /// <param name="validationResult">The validation result object.</param>
        protected ValidationResult(ValidationResult validationResult)
        {
            this.ErrorMessage = validationResult.ErrorMessage;
            this.MemberNames = validationResult.MemberNames;
        }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class by using an error message
        /// and a list of members that have validation errors.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The list of member names that have validation errors.</param>
        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = memberNames;
        }

        /// <summary>
        /// Prepends a name separated by a dot '.' to all items in MemberNames
        /// </summary>
        /// <param name="prepend"></param>
        public void PrependMemberNames(string prepend)
        {
            var newMembers = new List<string>();
            foreach (var member in this.MemberNames)
            {
                newMembers.Add(prepend + "." + member);
            }
            this.MemberNames = newMembers;
        }

        /// <summary>
        /// Returns a string representation of the current validation result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
