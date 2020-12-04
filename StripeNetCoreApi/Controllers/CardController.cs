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
    public class CardController : BaseApiController
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }
        [HttpPost("AddCard")]
        public IActionResult AddCard(StripeCardDTO dto)
        {
            var userSession = (UserSession)HttpContext.Items["usersession"];
            var response = _cardService.Create(dto, userSession.UserId);
            if (response.HasError)
                return Error(response);
            return Ok(response);
        }
        [HttpGet("GetCardsbyUserId")]
        public IActionResult GetCardsbyUserId()
        {
            var userSession = (UserSession)HttpContext.Items["usersession"];
            var response = _cardService.GetCardListByUserId(userSession.UserId);
            if (response.HasError)
                return Error(response);
            return Ok(response);
        }
    }
}
