using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YuYan.Domain.DTO;
using YuYan.Interface.Service;

namespace YuYan.API.Controllers
{
    [RoutePrefix("client")]
    public class ClientController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public ClientController(IYuYanService yuyanSvc)
        {
            _yuyanSvc = yuyanSvc;
        }

        [Route("{urltoken}"), HttpGet]
        public async Task<IHttpActionResult> GetSurveyByURLToken(string urltoken)
        {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try
            {
                dtoSurvey = await _yuyanSvc.GetSurveyByURLToken(urltoken);
                if (dtoSurvey == null)
                    return NotFound();
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoSurvey);
        }
    }
}
