using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using StripeNetCoreApi.DTO.ErrorDTO;
using StripeNetCoreApi.DTO.ResponseDTO;

namespace StripeNetCoreApi.Controllers.Base
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult InternalError(Response response)
        {
            return Error(response, System.Net.HttpStatusCode.InternalServerError);
        }

        protected IActionResult Error(Response response)
        {
            return Error(response, System.Net.HttpStatusCode.BadRequest);
        }

        protected IActionResult Error(Response response, object DTO)
        {
            return Error(response, System.Net.HttpStatusCode.BadRequest);
        }

        protected IActionResult Error(Response response, System.Net.HttpStatusCode statusCode)
        {
            ErrorDTO error = new ErrorDTO();
            error.Message = response.ErrorMessage;

            return new ErrorResult(statusCode, error);
        }
    }
    class ErrorResult : IActionResult
    {
        System.Net.Http.HttpResponseMessage response;

        public ErrorResult(System.Net.HttpStatusCode statusCode, ErrorDTO error)
        {
            error.Status = (int)statusCode;
            response = new System.Net.Http.HttpResponseMessage(statusCode)
            {
                Content = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(error, Newtonsoft.Json.Formatting.None), System.Text.Encoding.UTF8, "application/json")
            };
        }


        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)response.StatusCode;

            foreach (var header in response.Headers)
            {
                context.HttpContext.Response.Headers.TryAdd(header.Key, new StringValues(header.Value.ToArray()));
            }

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                await stream.CopyToAsync(context.HttpContext.Response.Body);
                await context.HttpContext.Response.Body.FlushAsync();
            }
        }
    }
}