using StripeNetCoreApi.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.ResponseDTO
{
    public class Response
    {
        /// <summary>
        /// Whether the operation completed successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTTP response code (4xx/5xx) if applicable.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Checks if response has errors
        /// </summary>
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMessage); } }

        private List<ValidationResult> _validationResults = new List<ValidationResult>();
        /// <summary>
        /// List of validation errors
        /// </summary>
        public IReadOnlyList<ValidationResult> ValidationResults { get { return _validationResults; } }

        /// <summary>
        /// Validates the model based on validation attributes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ValidateModel(object obj)
        {
            _validationResults.Clear();
            _validationResults.AddRange(Validation.GetValidationResults(obj));
            ErrorMessage = string.Join(", ", ValidationResults.Select(v => v.ErrorMessage));
            return _validationResults.Count == 0;
        }

        /// <summary>
        /// Copies properties from response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="property"></param>
        public Response Copy(Response response, string property = null)
        {
            CopyState(response);
            return this;
        }

        protected void CopyState(Response response, string property = null)
        {
            this.Success = response.Success;
            this.ErrorMessage = response.ErrorMessage;
            foreach (var valError in response.ValidationResults)
            {
                var memberNames = new List<string>();
                foreach (var member in valError.MemberNames)
                    memberNames.Add(string.IsNullOrEmpty(property) ? member : property + "." + member);
                this._validationResults.Add(new ValidationResult(valError.ErrorMessage, memberNames));
            }
        }

        /// <summary>
        /// Sets the response to a failed state
        /// </summary>
        /// <param name="response"></param>
        public Response Fail(string errorMessage)
        {
            SetFailState(errorMessage);
            return this;
        }

        protected void SetFailState(string errorMessage)
        {
            this.Success = false;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Response AddValidationError(string property, string message)
        {
            SetValidationError(property, message);
            return this;
        }

        protected void SetValidationError(string property, string message)
        {
            this.Success = false;
            this._validationResults.Add(new ValidationResult(message, new string[] { property }));
            if (string.IsNullOrEmpty(this.ErrorMessage))
                this.ErrorMessage = message;
        }
    }

    public class Result<T>
    {
        public Result()
        {
            Data = new List<T>();
        }

        /// <summary>
        /// The outout if the operation
        /// </summary>
        public List<T> Data { get; set; }
        /// <summary>
        /// Total count of data
        /// </summary>
        public long Count { get; set; }
    }

    public class Response<T> : Response
    {
        /// <summary>
        /// The output of the operation
        /// </summary>
        public T Data { get; set; }
        public string PageIndex { get; set; }
        /// <summary>
        /// Copies properties from response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="property"></param>
        public new Response<T> Copy(Response response, string property = null)
        {
            CopyState(response, property: property);
            return this;
        }

        /// <summary>
        /// Sets the response to a failed state
        /// </summary>
        /// <param name="response"></param>
        public new Response<T> Fail(string errorMessage)
        {
            SetFailState(errorMessage);
            return this;
        }

        /// <summary>
        /// Adds validation error and sets to a failed state
        /// </summary>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public new Response<T> AddValidationError(string property, string message)
        {
            SetValidationError(property, message);
            return this;
        }
    }
}
