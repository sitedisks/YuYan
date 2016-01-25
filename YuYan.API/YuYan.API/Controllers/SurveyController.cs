using System;
using System.Threading.Tasks;
using System.Web.Http;
using YuYan.Domain.DTO;
using YuYan.Interface.Service;

namespace YuYan.API.Controllers
{
    [RoutePrefix("survey")]
    public class SurveyController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public SurveyController(IYuYanService yuyanSvc) {
            _yuyanSvc = yuyanSvc;
        }

        [Route("{id}"), HttpGet]
        public async Task<IHttpActionResult> GetSurveyBySurveyId(int surveyId) {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try
            {
                dtoSurvey = await _yuyanSvc.GetSurveyBySurveyId(surveyId);
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

        [Route("user/{userid}"), HttpGet]
        public IHttpActionResult GetAllSurveysByUserId(Guid userId) {

            return Ok();
        }
    }
}
