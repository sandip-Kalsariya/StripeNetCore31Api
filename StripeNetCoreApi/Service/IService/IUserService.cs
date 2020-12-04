using Stripe;
using StripeNetCoreApi.DTO.RequestDTO;
using StripeNetCoreApi.DTO.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.IService
{
    public interface IUserService
    {
        Response<UserProfileDTO> UserRegistration(UserRegistrationDTO dto);
        Response<ServiceProviderDTO> ServiceProviderRegistration(ServiceProviderRegistrationDTO dto);
        Response<UserProfileDTO> Authenticate(LoginDTO dto);
        Response<string> PayFromUser(int UserId);
    }
}
