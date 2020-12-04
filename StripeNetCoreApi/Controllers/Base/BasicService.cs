using StripeNetCoreApi.DTO.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace StripeNetCoreApi.Controllers.Base
{
    public class BasicService
    {
        protected void HandleException(Response response, Exception ex)
        {
            if (response != null)
            {
                response.ErrorMessage = ex.Message;
            }
        }
    }
}
