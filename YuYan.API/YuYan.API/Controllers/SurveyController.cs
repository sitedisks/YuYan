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

        #region survey
        [Route("{surveyid}"), HttpGet]
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

        [HttpPost]
        public async Task<IHttpActionResult> CreateSurvey([FromBody] dtoSurvey survey) {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try {
                dtoSurvey = await _yuyanSvc.CreateSurvey(survey);        
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

        [HttpPut]
        public async Task<IHttpActionResult> UpdateSurvey([FromBody] dtoSurvey survey)
        {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try {
                dtoSurvey = await _yuyanSvc.UpdateSurvey(survey);
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

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSurvey(int surveyId) {
            try {
                await _yuyanSvc.DeleteSurvey(surveyId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok();
        }



        [Route("user/{userid}"), HttpGet]
        public IHttpActionResult GetAllSurveysByUserId(Guid userId) {

            return Ok();
        }
        #endregion

        #region question
        #endregion

        #region item
        #endregion
    }
}
