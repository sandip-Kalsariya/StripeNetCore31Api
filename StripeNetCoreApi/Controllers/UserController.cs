using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Service.IService;

namespace StripeNetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("UserRegister")]
        public IActionResult UserRegister([FromBody] UserRegistrationDTO dto)
        {
            var _AddUser = _userService.UserRegistration(dto);
            if (_AddUser.HasError)
                return Error(_AddUser);
            return Ok(_AddUser);
        }

        [HttpPost("InfluencerRegister")]
        public IActionResult ServiceProviderRegistration([FromBody] ServiceProviderRegistrationDTO dto)
        {
            var _AddInfluencer = _userService.ServiceProviderRegistration(dto);
            if (_AddInfluencer.HasError)
                return Error(_AddInfluencer);
            return Ok(_AddInfluencer);
        }

        [Authorize]
        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var usersession = (UserSession)HttpContext.Items["usersession"];
            if (usersession.User.RoleId != 3)
                return Unauthorized();
            var response = _userService.PayFromUser(usersession.UserId);
            if (response.HasError)
                return Error(response);
            return Ok(response);
        }
    }
}
