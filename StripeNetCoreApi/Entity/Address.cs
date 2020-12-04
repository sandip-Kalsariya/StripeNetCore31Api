using Newtonsoft.Json;
using StripeNetCoreApi.Generic.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Entity
{
    public class Address : BaseEntity
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
    }
}
