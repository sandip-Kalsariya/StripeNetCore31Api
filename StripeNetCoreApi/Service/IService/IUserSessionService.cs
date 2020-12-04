using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service.IService
{
    public interface IUserSessionService
    {
        Response<UserSession> Create(UserSession UserSessions);
        Response<UserSession> GetUserSessionByAccessToken(string token);
        Response<UserSession> Update(UserSession UserSessions);
    }
}
