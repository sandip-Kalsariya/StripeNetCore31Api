using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DataAnnotations
{
    public class DisplayAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value that is used for display in the UI.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value that is to display the short version name in the UI.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the type that contains the resources for the DisplayAttribute.Name and DisplayAttribute.ShortName properties.
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        /// Returns a value that is used for field display in the UI.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (ResourceType == null)
                return Name;

            return GetString(Name);
        }

        /// <summary>
        /// Returns the value of the DisplayAttribute.ShortName property.
        /// </summary>
        /// <returns></returns>
        public string GetShortName()
        {
            if (ResourceType == null)
                return ShortName;

            return GetString(ShortName);
        }

        private string GetString(string name)
        {
            var prop = ResourceType.GetRuntimeProperty(name);
            if (prop == null)
                throw new InvalidOperationException(
                    string.Format("The type {0} does not contain a property named {1}", ResourceType.FullName, name));
            var value = prop.GetValue(null);
            if (value == null) return null;
            return value.ToString();
        }
    }
}
