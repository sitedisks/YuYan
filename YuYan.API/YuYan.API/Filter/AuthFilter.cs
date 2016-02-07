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
    public class AuthFilter : ActionFilterAttribute
    {
        public IYuYanService _yuyanSvc;
        public const string AUTH_HEADER = "yuyan.header.token";
        public bool AllowAnonymous = false;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestScope = actionContext.Request.GetDependencyScope();
            _yuyanSvc = requestScope.GetService(typeof(IYuYanService)) as IYuYanService;

            try
            {
                string token = string.Empty;
                Guid session = Guid.Empty;

                if (!AllowAnonymous)
                {
                    if (!actionContext.Request.Headers.Contains(AUTH_HEADER))
                        throw new UnauthorizedAccessException("Missing required security header!");
                }

                if (actionContext.Request.Headers.Contains(AUTH_HEADER))
                {
                    token = actionContext.Request.Headers.FirstOrDefault(h => h.Key == AUTH_HEADER).Value.First();

                    if (!Guid.TryParse(token, out session))
                        throw new UnauthorizedAccessException("Invalid security token!");

                    var sessionObj = _yuyanSvc.ValidateSession(session);
                    if (sessionObj == null)
                        throw new UnauthorizedAccessException("Invalid session!");
                    else if(sessionObj.SessionId == Guid.Empty) {
                        throw new UnauthorizedAccessException("Session expired!");
                    }

                    var user = _yuyanSvc.GetUserBySessionId(session);
                    if (user == null)
                        throw new UnauthorizedAccessException("Invalid user!");
                }

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

        public void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //throw new NotImplementedException();
        }
    }
}