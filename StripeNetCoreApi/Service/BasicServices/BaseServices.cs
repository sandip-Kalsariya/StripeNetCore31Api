using Stripe;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.BasicServices
{
    public class BaseServices : BasicService, IBaseService
    {
        private IUserService userService;
        protected IUserService _userService { get { return userService; } }
        public BaseServices()
        {

        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public Customer CreateStripCustomerAccount(CustomerCreateOptions options)
        {
            var response = new Customer();
            try
            {

                var service = new CustomerService();
                response = service.Create(options);
            }
            catch (Exception)
            {
                throw;
            }
            return response;

        }

        public StripeList<Card> GetCardList(string customerid)
        {
            var service = new Stripe.CardService();
            var options = new CardListOptions
            {
                Limit = 50,
            };
            var cards = service.List(customerid, options);
            return cards;
        }

        public Response<Charge> Payment(PaymentDTO dto)
        {
            var response = new Response<Charge>();
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = dto.Amount,
                    Currency = dto.Currency,
                    Customer = dto.CustomerId,
                    Source = dto.CardId,
                    Metadata = dto.Metadata,
                    Description = dto.Desciption,
                    Shipping = new ChargeShippingOptions
                    {
                        Name = "Payment",
                        Address = new AddressOptions
                        {
                            City = dto.address.City,
                            Country = dto.address.Country,
                            State = dto.address.State,
                            Line1 = dto.address.Line1,
                            Line2 = dto.address.Line2,
                            PostalCode = dto.address.PostalCode
                        },
                    }
                };
                var service = new ChargeService();
                var returndata = service.Create(options);
                response.Data = returndata;
                if (returndata.Status == "succeeded")
                    response.Success = true;
            }
            catch (StripeException e)
            {
                response.AddValidationError("Message", e.StripeError.Message);
                return response;
            }
            return response;
        }
        public Response<Refund> Refund(string ChargeId)
        {
            var response = new Response<Refund>();
            try
            {
                var options = new RefundCreateOptions
                {
                    Charge = ChargeId
                };
                var service = new RefundService();
                service.Create(options);
            }
            catch (StripeException ex)
            {
                response.AddValidationError("", ex.StripeError.Message);
                return response;
            }
            return response;
        }
   
        public Card CreateCartdStripe(StripeCardDTO dto, string customerid)
        {
            var tokenCreate = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = dto.Cardnumber,
                    ExpMonth = dto.Month,
                    ExpYear = dto.Year,
                    Cvc = dto.CVC,
                    Name = dto.CardHolderName
                },
            };
            var tokenService = new TokenService();
            var token = tokenService.Create(tokenCreate);

            var options = new CardCreateOptions
            {
                Source = token.Id
            };
            var service = new Stripe.CardService();
            var response = service.Create(customerid, options);
            return response;
        }

        public void SetAccountService(IUserService UserService)
        {
            userService = UserService;
        }
        public void Dispose()
        {

        }

    }
}
