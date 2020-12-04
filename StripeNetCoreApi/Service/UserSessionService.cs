using StripeNetCoreApi.Controllers.Base;
using StripeNetCoreApi.DTO.ResponseDTO;
using StripeNetCoreApi.Entity;
using StripeNetCoreApi.Repository.IRepository;
using StripeNetCoreApi.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.Service
{
    public class UserSessionService : BasicService, IUserSessionService
    {
        private readonly IUserSessionRepository _userSessionRepository;
        public UserSessionService(IUserSessionRepository userSessionRepository)
        {
            _userSessionRepository = userSessionRepository;
        }
        public Response<UserSession> Create(UserSession UserSessions)
        {
            var response = new Response<UserSession>();
            try
            {
                UserSession newObj = new UserSession
                {
                    AccessToken = UserSessions.AccessToken,
                    UserId = UserSessions.UserId,
                    CreationTime = DateTime.UtcNow.ToString(),
                };
                _userSessionRepository.Insert(newObj);
                response.Success = true;
                response.Data = newObj;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

        public Response<UserSession> Update(UserSession UserSessions)
        {
            var response = new Response<UserSession>();
            try
            {
                var session = _userSessionRepository.GetById(a => a.Id == UserSessions.Id && a.DateDeleted == null);
                if (session == null)
                {
                    response.AddValidationError("", "Session notFound.");
                    return response;
                }
                session.LastModificationTime = DateTime.UtcNow.ToString();
                _userSessionRepository.Update(session);
                response.Success = true;
                response.Data = session;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }
        public Response<UserSession> GetUserSessionByAccessToken(string token)
        {
            var response = new Response<UserSession>();
            try
            {
                var _userSession = _userSessionRepository.GetbyidwithInclude(a => a.AccessToken == token && a.DateDeleted == null, "User");
                if (_userSession == null)
                {
                    response.AddValidationError("", "Session Token doesnot exist.");
                    return response;
                }
                response.Success = true;
                response.Data = _userSession;
            }
            catch (Exception ex)
            {
                HandleException(response, ex);
            }
            return response;
        }

    }
}
