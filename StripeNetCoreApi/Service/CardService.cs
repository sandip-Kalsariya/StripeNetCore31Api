using Stripe;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Repository.IRepository;
using StripeNetCoreApi.Service.BasicServices;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service
{
    public class CardService : BasicService, ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly BaseServices _baseServices;
        private readonly IUserRepository _userRepository;

       

        public CardService(ICardRepository cartRepository, BaseServices baseServices, IUserRepository userRepository)
        {
            _cardRepository = cartRepository;
            _baseServices = baseServices;
            _userRepository = userRepository;
        }

        public Response<string> Create(StripeCardDTO dto, int UserId)
        {
            var response = new Response<string>();
            try
            {
                var User = _userRepository.GetById(a => a.Id == UserId && a.DateDeleted == null);
                if (User == null)
                {
                    response.AddValidationError("", "User doesnot exist.");
                    return response;
                }
                var card = _baseServices.CreateCartdStripe(dto, User.Stripe_CustomerId);
                Entity.Card cart = new Entity.Card();
                cart.UserId = UserId;
                cart.StripeCardId = card.Id;
                cart.DateCreated = DateTime.UtcNow.ToString();
                _cardRepository.Insert(cart);

                response.Data = card.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public Response<StripeList<Stripe.Card>> GetCardListByUserId(int UserId)
        {
            var response = new Response<StripeList<Stripe.Card>>();
            try
            {
                var _user = _userRepository.GetById(a => a.Id == UserId && a.DateDeleted == null);
                if (_user == null)
                {
                    response.AddValidationError("", "User doesnot exist.");
                    return response;
                }
                var _stripecarts = _baseServices.GetCardList(_user.Stripe_CustomerId);
                response.Data = _stripecarts;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public Response VerifyCard(int UserId, string CardNumber)
        {
            var response = new Response();
            try
            {

            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
    }
}
