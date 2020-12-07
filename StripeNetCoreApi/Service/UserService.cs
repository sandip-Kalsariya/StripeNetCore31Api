using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Helpers;
using StripeNetCoreApi.Repository.IRepository;
using Stripe;
using StripeNetCoreApi.Service.BasicServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using StripeNetCoreApi.DTO.EnumDTO;

namespace StripeNetCoreApi.Service
{
    public class UserService : BasicService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly BaseServices _baseServices;
        private readonly IUserSessionService _userSessionService;
        private readonly AppSettings _appSettings;
        private readonly IAddressSerevice _addressSerevice;
        private readonly IOrderRepository _orderRepository;
        private readonly ICardService _cardService;
        private readonly ICardRepository _cardRepository;


        public UserService(
            IUserRepository userRepository,
            BaseServices baseServices,
            IUserSessionService userSessionService,
            IOptions<AppSettings> appSettings,
            IAddressSerevice addressSerevice,
            IOrderRepository orderRepository,
            ICardService cardService,
            ICardRepository cardRepository
            )
        {
            _userRepository = userRepository;
            _baseServices = baseServices;
            _userSessionService = userSessionService;
            _appSettings = appSettings.Value;
            _addressSerevice = addressSerevice;
            _orderRepository = orderRepository;
            _cardService = cardService;
            _cardRepository = cardRepository;
        }

        public Response<UserProfileDTO> UserRegistration(UserRegistrationDTO dto)
        {
            var response = new Response<UserProfileDTO>();
            try
            {
                User user = new User();
                user.Name = dto.Name;
                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.Password = Helper.Encrypt(dto.Password);
                user.Phone_Number = dto.PhoneNumber;
                user.RoleId = 3;
                user.DateCreated = DateTime.UtcNow.ToString();

                if (_userRepository.ListQuery(a => a.DateDeleted == null && !string.IsNullOrEmpty(a.Email) && !string.IsNullOrEmpty(user.Email) && a.Email.ToLower() == user.Email.ToLower()).Any())
                {
                    response.AddValidationError("", "Email is already exist!");
                    return response;
                }
                if (_userRepository.ListQuery(a => a.DateDeleted == null && !string.IsNullOrEmpty(a.UserName) && !string.IsNullOrEmpty(user.UserName) && a.UserName.ToLower() == user.UserName.ToLower()).Any())
                {
                    response.AddValidationError("", "UserName is already exist!");
                    return response;
                }

                var customerCreateOptions = new CustomerCreateOptions();
                customerCreateOptions.Email = user.Email;
                customerCreateOptions.Name = user.Name;
                customerCreateOptions.Address = new AddressOptions
                {
                    City = dto.Address.City,
                    State = dto.Address.State,
                    Country = dto.Address.Country,
                    PostalCode = dto.Address.PostalCode,
                    Line1 = dto.Address.Line1,
                    Line2 = dto.Address.Line2
                };

                var addressRes = _addressSerevice.Create(dto.Address);
                if (addressRes.HasError)
                {
                    response.AddValidationError("", addressRes.ErrorMessage);
                    return response;
                }
                user.AddressId = addressRes.Data.Id;

                var customer = _baseServices.CreateStripCustomerAccount(customerCreateOptions);
                user.Stripe_CustomerId = customer.Id;

                _userRepository.Insert(user);

                var token = generateJwtToken(user);
                _userSessionService.Create(new UserSession
                {
                    AccessToken = token,
                    UserId = user.Id
                });

                var userResponse = _userRepository.GetbyidwithInclude(a => a.Id == user.Id && a.DateDeleted == null, "Roles,Address");
                UserProfileDTO registrationResponse = new UserProfileDTO();
                registrationResponse.Id = userResponse.Id;
                registrationResponse.Name = userResponse.Name;
                registrationResponse.JwtToken = token;
                registrationResponse.Role = userResponse.Roles?.Name;
                registrationResponse.RoleId = userResponse.RoleId;
                registrationResponse.Customer_id = userResponse.Stripe_CustomerId;
                registrationResponse.UserName = userResponse.UserName;
                registrationResponse.Email = userResponse.Email;
                registrationResponse.Birthdate = userResponse.Birthdate;
                registrationResponse.Phone_Number = userResponse.Phone_Number;

                response.Data = registrationResponse;
                response.Success = true;

            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public Response<ServiceProviderDTO> ServiceProviderRegistration(ServiceProviderRegistrationDTO dto)
        {
            var response = new Response<ServiceProviderDTO>();
            try
            {
                User user = new User();
                user.Name = dto.Name;
                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.Password = Helper.Encrypt(dto.Password);
                user.Birthdate = dto.Birthday;
                user.Phone_Number = dto.PhoneNumber;
                user.RoleId = 2;
                user.DateCreated = DateTime.UtcNow.ToString();
                user.Address = dto.Address;
                if (_userRepository.ListQuery(a => a.DateDeleted == null && !string.IsNullOrEmpty(a.Email) && !string.IsNullOrEmpty(user.Email) && a.Email.ToLower() == user.Email.ToLower()).Any())
                {
                    response.AddValidationError("", "Email is already exist!");
                    return response;
                }
                if (_userRepository.ListQuery(a => a.DateDeleted == null && !string.IsNullOrEmpty(a.UserName) && !string.IsNullOrEmpty(user.UserName) && a.UserName.ToLower() == user.UserName.ToLower()).Any())
                {
                    response.AddValidationError("", "UserName is already exist!");
                    return response;
                }
                var customerCreateOptions = new CustomerCreateOptions();

                //Create Stripe Customer Accout

                customerCreateOptions.Email = user.Email;
                customerCreateOptions.Name = user.Name;
                customerCreateOptions.Address = new AddressOptions
                {
                    City = user.Address.City,
                    State = user.Address.State,
                    Country = user.Address.Country,
                    PostalCode = user.Address.PostalCode,
                    Line1 = user.Address.Line1,
                    Line2 = user.Address.Line2
                };

                var customer = _baseServices.CreateStripCustomerAccount(customerCreateOptions);
                user.Stripe_CustomerId = customer.Id;

                _userRepository.Insert(user);
                var token = generateJwtToken(user);
                _userSessionService.Create(new UserSession
                {
                    AccessToken = token,
                    UserId = user.Id
                });

                var userResponse = _userRepository.GetbyidwithInclude(a => a.Id == user.Id && a.DateDeleted == null, "Roles,Address");

                ServiceProviderDTO registrationResponse = new ServiceProviderDTO();
                registrationResponse.Id = userResponse.Id;
                registrationResponse.Name = userResponse.Name;
                registrationResponse.JwtToken = token;
                registrationResponse.Role = userResponse.Roles?.Name;
                registrationResponse.RoleId = userResponse.RoleId;
                registrationResponse.Customer_id = userResponse.Stripe_CustomerId;
                registrationResponse.UserName = userResponse.UserName;
                registrationResponse.Email = userResponse.Email;
                registrationResponse.Phone_Number = userResponse.Phone_Number;
                response.Data = registrationResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public Response<UserProfileDTO> Authenticate(LoginDTO dto)
        {
            var response = new Response<UserProfileDTO>();
            var user = new User();
            try
            {

                var pass = Helper.Encrypt(dto.Password);
                dto.Password = pass;
                User _user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password
                };
                user = _userRepository.UserLogin(_user);
                if (user == null)
                {
                    response.AddValidationError("", "Please Enter valid email address or password.");
                    return response;
                }
                _userRepository.Update(user);
                var token = generateJwtToken(user);
                _userSessionService.Create(new UserSession
                {
                    AccessToken = token,
                    UserId = user.Id
                });
                var userResponse = _userRepository.GetbyidwithInclude(a => a.Id == user.Id && a.DateDeleted == null, "Roles,Address");


                UserProfileDTO registrationResponse = new UserProfileDTO();
                registrationResponse.Id = userResponse.Id;
                registrationResponse.Name = userResponse.Name;
                registrationResponse.JwtToken = token;
                registrationResponse.Role = userResponse.Roles?.Name;
                registrationResponse.RoleId = userResponse.RoleId;
                registrationResponse.Customer_id = userResponse.Stripe_CustomerId;
                registrationResponse.UserName = userResponse.UserName;
                registrationResponse.Email = userResponse.Email;
                registrationResponse.Birthdate = userResponse.Birthdate;
                registrationResponse.Phone_Number = userResponse.Phone_Number;

                response.Data = registrationResponse;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Response<string> PayFromUser(PaymentRequestDTO dto, int UserId)
        {
            var response = new Response<string>();
            string CardId = null;
            try
            {
                var Order = _orderRepository.GetbyidwithInclude(a => a.Id == dto.PromoOrderId && a.DateDeleted == null, "User");
                if (Order == null)
                {
                    response.AddValidationError("", "Order doesnot exist.");
                    return response;
                }

                if (dto.CardId == "" || dto.CardId == null)
                {
                    var cardresponse = _cardService.Create(dto.CardDetail, UserId);
                    if (cardresponse.HasError)
                    {
                        response.ErrorMessage = cardresponse.ErrorMessage;
                        return response;
                    }
                    CardId = cardresponse.Data;
                }
                else
                {
                    CardId = _cardRepository.GetById(a => a.StripeCardId == dto.CardId && a.DateDeleted == null).StripeCardId;
                }

                var userResponse = _userRepository.GetbyidwithInclude(a => a.Id == UserId && a.DateDeleted == null, "Roles,Address");
                if (userResponse == null)
                {
                    response.AddValidationError("", "PromoOrder doesnot exist.");
                    return response;
                }

                //Add Charge
                double amt = Order.Net + Order.Fee;
                double centamt = amt * 100;
                double charge = (centamt * 2.9 / 100);  // 2.9 % of service price
                double centchanrge = (0.3 * 100);        // 0.30 Fixed Charge
                double totalCharg = charge + centchanrge;
                double finalamount = centamt + totalCharg;

                var responsepayment = _baseServices.Payment(new PaymentDTO
                {
                    Amount = Convert.ToInt64(Math.Ceiling(finalamount)),
                    Currency = "usd",
                    CustomerId = userResponse.Stripe_CustomerId,
                    CardId = CardId,
                    Desciption = "Pay for Test",
                    Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", Order.Id.ToString()},
                        { "Service Title", ""},
                        { "Discount", Order.Discount.ToString() },
                        { "Fee", Order.Fee.ToString() }
                    },
                    address = userResponse.Address
                });
                if (!responsepayment.Success)
                {
                    response.ErrorMessage = responsepayment.ErrorMessage;
                    return response;
                }

                if (responsepayment.Success && responsepayment.Data != null && responsepayment.Data.Status == "succeeded")
                {
                    Order.PaymentStaus = PaymentStaus.Completed;
                }
                else if (responsepayment.Success && responsepayment.Data != null && responsepayment.Data.Status == "pending")
                {
                    Order.PaymentStaus = PaymentStaus.Pending;
                }
                else if (responsepayment.Success && responsepayment.Data != null && responsepayment.Data.Status == "failed")
                {
                    Order.PaymentStaus = PaymentStaus.Reject;
                }

                Order.ChargeId = responsepayment.Data.Id;

                _orderRepository.Update(Order);

                response.Data = responsepayment.Data.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        //public Response CancledPromoOrder(int UserId, CancledPromoOrderDTO dto)
        //{
        //    var response = new Response();
        //    try
        //    {
        //        var promoorder = _promoOrderRepository.GetbyidwithInclude(a => a.Id == dto.PromoOrderId && a.DateDeleted == null, "InfluncerServices,User,Cart"); //a.PromoStatus != PromoStatus.Completed &&
        //        if (promoorder == null)
        //        {
        //            response.AddValidationError("", "Order Doesnot Exist.");
        //            response.Status = 406;
        //            return response;
        //        }
        //        if (dto.Status == OrderStatus.CancledByInfluncer)
        //        {
        //            if (promoorder.Status == dto.Status)
        //            {
        //                response.AddValidationError("", "Order Already Cancled By Influncer.");
        //                response.Status = 406;
        //                return response;
        //            }
        //            promoorder.Status = OrderStatus.CancledByInfluncer;
        //            promoorder.Reason_NotAbleComplete = dto.ResionForCancellation;
        //            if (promoorder.PaymentStaus == PaymentStaus.Completed)
        //            {
        //                // *********** Add Brand Refund Service. *************//
        //                var refundresponse = _baseServices.Refund(promoorder.ChargeId);
        //                if (refundresponse.HasError)
        //                    return refundresponse;

        //                promoorder.PaymentStaus = PaymentStaus.Refund;
        //            }
        //        }
        //        else if (dto.Status == OrderStatus.CancledByBrand)
        //        {
        //            if (promoorder.Status == dto.Status)
        //            {
        //                response.AddValidationError("", "Order Already Cancled By Brand.");
        //                response.Status = 406;
        //                return response;
        //            }
        //            promoorder.Status = OrderStatus.CancledByBrand;
        //            promoorder.Reason_NotAbleComplete = dto.ResionForCancellation;
        //        }

        //        promoorder.DateModified = DateTime.UtcNow.ToString();
        //        _promoOrderRepository.Update(promoorder);
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(response, ex);
        //    }
        //    return response;
        //}

    }
}
