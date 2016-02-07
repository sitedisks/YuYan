using System;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using YuYan.Interface.Service;
using YuYan.Domain.DTO;
using System.Net;
using YuYan.API.Filter;


namespace YuYan.API.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public UserController(IYuYanService yuyanSvc)
        {
            _yuyanSvc = yuyanSvc;
        }

        [Route("ipaddress"), HttpGet]
        public string GetIP()
        {
            return GetClientIp();
        }

        #region user
        [Route("check"), HttpPost]
        public async Task<IHttpActionResult> CheckUser(dtoUser user)
        {
            dtoUser userObj = null;
            try
            {
                userObj = await _yuyanSvc.CheckUserAvailability(user.Email);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(userObj);
        }

        [Route("register"), HttpPost]
        public async Task<IHttpActionResult> Register(dtoUser user)
        {
            dtoUserProfile userProfile = new dtoUserProfile();

            try
            {
                user.IPAddress = GetClientIp();
                userProfile = await _yuyanSvc.RegisterNewUser(user);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(userProfile);
        }

        [Route("login"), HttpPost]
        public async Task<IHttpActionResult> Login(dtoUser user)
        {
            dtoUserProfile userProfile = new dtoUserProfile();

            try
            {
                user.IPAddress = GetClientIp();
                userProfile = await _yuyanSvc.LoginUser(user);

                if (userProfile == null)
                    return Content(HttpStatusCode.NotFound, "User not found.");

                if (userProfile.UserId == Guid.Empty)
                    return Content(HttpStatusCode.Unauthorized, "Username and Password not match.");
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(userProfile);
        }

        [Route("logout"), HttpDelete]
        public async Task<IHttpActionResult> Logout(Guid sessionId)
        {
            bool IsLogout = false;

            try
            {
                IsLogout = await _yuyanSvc.LogoutUser(sessionId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(IsLogout);
        }

        [Route("status"), HttpGet]
        public IHttpActionResult CheckSession(Guid sessionId) {
            dtoSession session = null;

            try {
                session = _yuyanSvc.ValidateSession(sessionId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(session);
        }

        [Route("update"), HttpPut]
        [AuthFilter]
        public async Task<IHttpActionResult> UpdateUserProfile(dtoUserProfile userProfile)
        {
            dtoUserProfile profile = null;

            try
            {
                userProfile.IPAddress = GetClientIp();
                profile = await _yuyanSvc.UpdateUserProfile(userProfile);
                if (profile == null)
                    return Content(HttpStatusCode.NotFound, "User not found.");
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(profile);
        }
        #endregion

        #region private functions
        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
