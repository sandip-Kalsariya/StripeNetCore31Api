using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.ErrorDTO
{
    public class ErrorDTO
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
