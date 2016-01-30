using System;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using YuYan.Interface.Service;
using YuYan.Domain.DTO;
using System.Net;


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

        #region user
        [Route("register"), HttpPost]
        public async Task<IHttpActionResult> Register(dtoUser user) {
            dtoUserProfile userProfile = new dtoUserProfile();

            try {
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
        public async Task<IHttpActionResult> Login(dtoUser user) {
            dtoUserProfile userProfile = new dtoUserProfile();

            try {
                user.IPAddress = GetClientIp();
                userProfile = await _yuyanSvc.LoginUser(user);

                if(userProfile == null)
                    return Content(HttpStatusCode.NotFound, "User not found.");

                if(userProfile.UserId == Guid.Empty)
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

        [Route("logout"), HttpPost]
        public async Task<IHttpActionResult> Logout(Guid sessionId) {
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
