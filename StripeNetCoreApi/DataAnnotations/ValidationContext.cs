using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StripeNetCoreApi.DataAnnotations
{
    public class ValidationContext
    {
        /// <summary>
        /// Gets the object to validate.
        /// </summary>
        public object ObjectInstance { get; private set; }

        /// <summary>
        /// Gets the dictionary of key/value pairs that is associated with this context.
        /// </summary>
        public IDictionary<string, object> Items { get; private set; }

        /// <summary>
        /// Gets or sets the name of the member to validate.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Initializes a new instance of the ValidationContext class using the specified object instance.
        /// </summary>
        /// <param name="instance"></param>
        public ValidationContext(object instance)
        {
            this.ObjectInstance = instance;
            this.Items = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the ValidationContext class using the specified object and an optional property bag.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="items"></param>
        public ValidationContext(object instance, IDictionary<string, object> items)
        {
            this.ObjectInstance = instance;
            this.Items = items;
        }

        /// <summary>
        /// Validates an object based on the results of all properties decorated with subclasses of ValidationAttribute
        /// and the results of IValidatableObject.Validate if the object implements this.
        /// </summary>
        /// <returns></returns>
        public IList<ValidationResult> GetValidationResults()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (ObjectInstance == null) return results;

            var type = ObjectInstance.GetType();
            var enumerableType = typeof(System.Collections.IEnumerable).GetTypeInfo();

            foreach (var prop in type.GetRuntimeProperties())
            {
                this.MemberName = prop.Name;
                if (!prop.CanWrite) continue;
                if (enumerableType.IsAssignableFrom(prop.PropertyType.GetTypeInfo()))
                {
                    var value = prop.GetValue(ObjectInstance);
                    if (value == null) continue;
                    var collection = (System.Collections.IEnumerable)value;
                    int count = 0;
                    foreach (var item in collection)
                    {
                        var subResults = new ValidationContext(item).GetValidationResults();
                        foreach (var result in subResults)
                        {
                            result.PrependMemberNames($"{this.MemberName}[{count}]");
                            results.Add(result);
                        }
                        count++;
                    }
                }
                else if (prop.PropertyType.FullName != "System.String" && prop.PropertyType.FullName != "Base1902.EncryptedString" && prop.PropertyType.GetTypeInfo().IsClass)
                {
                    var value = prop.GetValue(ObjectInstance);
                    var subResults = new ValidationContext(value).GetValidationResults();
                    foreach (var result in subResults)
                    {
                        result.PrependMemberNames(this.MemberName);
                        results.Add(result);
                    }
                }
                else
                {
                    foreach (var valAttr in prop.GetCustomAttributes<ValidationAttribute>())
                    {
                        if (valAttr.RequiresValidationContext)
                        {
                            var result = valAttr.GetValidationResult(prop.GetValue(ObjectInstance), this);
                            if (result != ValidationResult.Success)
                                results.Add(result);
                        }
                        else if (!valAttr.IsValid(prop.GetValue(ObjectInstance)))
                        {
                            string name = this.MemberName;
                            var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
                            if (displayAttr != null)
                                name = displayAttr.GetName();
                            results.Add(new ValidationResult(valAttr.FormatErrorMessage(name), new string[] { name }));
                        }
                    }
                }
            }

            if (ObjectInstance is IValidatableObject)
            {
                this.MemberName = null;
                foreach (var validationResult in ((IValidatableObject)ObjectInstance).Validate(this))
                {
                    results.Add(validationResult);
                }
            }

            return results;
        }
    }
}
