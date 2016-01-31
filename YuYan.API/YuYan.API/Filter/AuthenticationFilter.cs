using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using YuYan.Interface.Service;

namespace YuYan.API.Filter
{

    public class YYUser : IPrincipal
    {
        public bool Authenticatd { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity
        {
            get { return null; }
        }
    }

    public class AuthenticationFilter : AuthorizationFilterAttribute
    {
        public IYuYanService _yuyanSvc;
        public const string AUTH_HEADER = "yy-header-token";
        public bool AllowAnonymous = false;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var requestScope = actionContext.Request.GetDependencyScope();
            _yuyanSvc = requestScope.GetService(typeof(IYuYanService)) as IYuYanService;
            YYUser principle = new YYUser() { Authenticatd = false }; 

            try
            {
                string token = string.Empty; // token = key|{session}
                string key = string.Empty;
                Guid session = Guid.Empty;

                if (!actionContext.Request.Headers.Contains(AUTH_HEADER) && !AllowAnonymous)
                    throw new UnauthorizedAccessException("Missing required security header!");

                token = actionContext.Request.Headers.First(h => h.Key == AUTH_HEADER).Value.First();

                if (token.Split('|').Length != 2)
                {
                    if (!AllowAnonymous) { throw new UnauthorizedAccessException("Invalid security token!"); }
                }
                else
                {
                    key = token.Split('|')[0];
                    var strSession = token.Split('|')[1];

                    if (string.IsNullOrEmpty(key))
                        throw new UnauthorizedAccessException("Invalid seceurity key!");

                    if (!Guid.TryParse(strSession, out session))
                        throw new UnauthorizedAccessException("Invalid security token!");

                }

                var isExpired = _yuyanSvc.ValidateSession(session);
                if (isExpired && !AllowAnonymous)
                    throw new UnauthorizedAccessException("Invalid session!");

                var user = _yuyanSvc.GetUserBySessionId(session);
                if (user == null && !AllowAnonymous)
                    throw new UnauthorizedAccessException("Invalid user!");

                if (user != null) {
                    principle.Authenticatd = true;
                    principle.UserId = user.UserId;
                    principle.Username = user.Username;
                    principle.Email = user.Email;
                    principle.SessionId = session;
                }

                HttpContext.Current.User = principle;

            }
            catch (UnauthorizedAccessException uex)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                if (actionContext.Response.Headers.Contains(AUTH_HEADER))
                    actionContext.Response.Headers.Remove(AUTH_HEADER);

            }
            catch (Exception ex)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                if (actionContext.Response.Headers.Contains(AUTH_HEADER))
                    actionContext.Response.Headers.Remove(AUTH_HEADER);
            }
        }
    }
}