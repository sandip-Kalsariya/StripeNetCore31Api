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
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        public OrderController(
            IOrderService orderService
            )
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderRequestDTO dto)
        {
            //OrderRequestDTO dto = new OrderRequestDTO();
            //string DiscountCode = Request.Form["discountCode"].ToString();
            //if (DiscountCode != "")
            //{
            //    dto.DiscountCode = DiscountCode;
            //}
            //dto.Price = double.Parse(Request.Form["Price"]);
            //string discount = Request.Form["Discount"].ToString();
            //if (discount != "")
            //{
            //    dto.Discount = double.Parse(discount);
            //}
            //dto.Notes = Request.Form["notes"];
            var userSession = (UserSession)HttpContext.Items["usersession"];
            if (userSession.User.RoleId != 3)
                return Unauthorized();
            var response = await _orderService.Create(userSession.UserId, dto);
            if (response.HasError && response.Status == 406)
                return Error(response, System.Net.HttpStatusCode.NotAcceptable);
            return Ok(response);
        }

        [HttpGet("GetUserOrderList")]    //for Brand
        public IActionResult GetUserOrderList([FromQuery] Pagination dto)
        {
            var userSession = (UserSession)HttpContext.Items["usersession"];
            if (userSession.User.RoleId != 3)
                return Unauthorized();
            var response = _orderService.GetUserOrderList(userSession.UserId, dto);
            if (response.HasError)
                return Error(response);
            return Ok(response);
        }
    }
}
