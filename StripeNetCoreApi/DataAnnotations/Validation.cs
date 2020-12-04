using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DataAnnotations
{
    public sealed class Validation
    {
        public const string EmailRegEx = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public const string NameRegEx = @"^[ÆØÅæøåa-zA-Z][ÆØÅæøåa-z ,.'-]*$";

        /// <summary>
        /// Validates an object based on the results of all properties decorated with subclasses of ValidationAttribute
        /// and the results of IValidatableObject.Validate if the object implements this.
        /// </summary>
        /// <param name="obj">The </param>
        /// <returns></returns>
        public static IList<ValidationResult> GetValidationResults(object objectInstance)
        {
            if (objectInstance == null) return new List<ValidationResult>();

            var validationContext = new ValidationContext(objectInstance);
            return validationContext.GetValidationResults();
        }

        /// <summary>
        /// Validates EAN13.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ValidateEAN13(string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            int checkDigit = ChecksumEAN13(data.Substring(0, data.Length - 1));
            return checkDigit == int.Parse(data.Substring(data.Length - 1));
        }

        /// <summary>
        /// Computes check digit for EAN13.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ChecksumEAN13(string data)
        {
            // Test string for correct length
            if (data.Length != 12 && data.Length != 13)
                return -1;

            // Test string for being numeric
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < 0x30 || data[i] > 0x39)
                    return -1;
            }

            int sum = 0;

            for (int i = 11; i >= 0; i--)
            {
                int digit = data[i] - 0x30;
                if ((i & 0x01) == 1)
                    sum += digit * 3;
                else
                    sum += digit;
            }
            int mod = sum % 10;
            return mod == 0 ? 0 : 10 - mod;
        }

        /// <summary>
        /// Validates EAN8.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ValidateEAN8(string data)
        {
            if (string.IsNullOrEmpty(data)) return false;
            int checkDigit = ChecksumEAN8(data.Substring(0, data.Length - 1));
            return checkDigit == int.Parse(data.Substring(data.Length - 1));
        }

        /// <summary>
        /// Computes check digit for EAN8.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ChecksumEAN8(string data)
        {
            // Test string for correct length
            if (data.Length != 7 && data.Length != 8)
                return -1;

            // Test string for being numeric
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < 0x30 || data[i] > 0x39)
                    return -1;
            }

            int sum = 0;

            for (int i = 6; i >= 0; i--)
            {
                int digit = data[i] - 0x30;
                if ((i & 0x01) == 1)
                    sum += digit;
                else
                    sum += digit * 3;
            }
            int mod = sum % 10;
            return mod == 0 ? 0 : 10 - mod;
        }
    }
}
