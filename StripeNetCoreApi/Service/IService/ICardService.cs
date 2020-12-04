using Stripe;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.IService
{
    public interface ICardService
    {
        Response<string> Create(StripeCardDTO dto, int UserId);
        Response<StripeList<Stripe.Card>> GetCardListByUserId(int UserId);
        Response VerifyCard(int UserId, string CardNumber);
    }
}
