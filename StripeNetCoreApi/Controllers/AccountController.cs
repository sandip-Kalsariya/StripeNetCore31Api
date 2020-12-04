using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.Service.IService;

namespace StripeNetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDTO model)
        {
            var _loginResponse = _userService.Authenticate(model);
            if (_loginResponse.HasError)
                return Error(_loginResponse);
            setTokenCookie(_loginResponse.Data.JwtToken);
            return Ok(_loginResponse);
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
