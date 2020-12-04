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

namespace StripeNetCoreApi.Service
{
    public class UserService : BasicService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly BaseServices _baseServices;
        private readonly IUserSessionService _userSessionService;
        private readonly AppSettings _appSettings;
        private readonly IAddressSerevice _addressSerevice;

        public UserService(
            IUserRepository userRepository,
            BaseServices baseServices,
            IUserSessionService userSessionService,
            IOptions<AppSettings> appSettings,
            IAddressSerevice addressSerevice
            )
        {
            _userRepository = userRepository;
            _baseServices = baseServices;
            _userSessionService = userSessionService;
            _appSettings = appSettings.Value;
            _addressSerevice = addressSerevice;
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

        public Response<string> PayFromUser(int UserId)
        {
            var response = new Response<string>();
            string CardId = null;
            try
            {
                var userResponse = _userRepository.GetbyidwithInclude(a => a.Id == UserId && a.DateDeleted == null, "Roles,Address");
                if (userResponse == null)
                {
                    response.AddValidationError("", "PromoOrder doesnot exist.");
                    return response;
                }

                var responsepayment = _baseServices.Payment(new PaymentDTO
                {
                    Amount = Convert.ToInt64(Math.Ceiling(100.00)),
                    Currency = "usd",
                    CustomerId = userResponse.Stripe_CustomerId,
                    CardId = CardId,
                    Desciption = "Pay for Test",
                    Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", "1235698468"},
                        { "Service Title", "test"},
                        { "Discount", "0" },
                        { "Fee", "2" }
                    },
                    address = userResponse.Address
                });
                if (!responsepayment.Success)
                {
                    response.ErrorMessage = responsepayment.ErrorMessage;
                    return response;
                }

                response.Data = responsepayment.Data.Id;
                response.Success = true;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }


    }
}
