using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using YuYan.API.Filter;
using YuYan.Domain.DTO;
using YuYan.Interface.Service;

namespace YuYan.API.Controllers
{
    [RoutePrefix("report")]
    public class ReportController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public ReportController(IYuYanService yuyanSvc)
        {
            _yuyanSvc = yuyanSvc;
        }

        [Route("{surveyId}"),HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetClientDetailsBySurveyId(int surveyId)
        {

            IList<dtoSurveyClient> dtoClientList = new List<dtoSurveyClient>();

            try
            {
                dtoClientList = await _yuyanSvc.GetSurveyClientBySurveyId(surveyId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoClientList);
        }
    }
}
