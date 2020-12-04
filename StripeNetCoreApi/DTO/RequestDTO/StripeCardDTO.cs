using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.RequestDTO
{
    public class StripeCardDTO
    {
        public string Cardnumber { get; set; }
        public string CVC { get; set; }
        public string CardHolderName { get; set; }
        public long Year { get; set; }
        public long Month { get; set; }
    }
}
