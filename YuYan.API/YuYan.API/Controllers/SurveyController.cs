using System;
using System.Threading.Tasks;
using System.Web.Http;
using YuYan.Domain.DTO;
using YuYan.Interface.Service;

namespace YuYan.API.Controllers
{
    [RoutePrefix("surveys")]
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

        [Route(""), HttpPost]
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

        [Route(""), HttpPut]
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

        [Route("{surveyid}"), HttpDelete]
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

        /*
        [Route("user/{userid}"), HttpGet]
        public IHttpActionResult GetAllSurveysByUserId(Guid userId) {

            return Ok();
        }
         */
        #endregion

        #region question
        [Route("{surveyid}/questions"), HttpGet]
        public async Task<IHttpActionResult> GetQuestionBySurveyId(int surveyId) {

            return Ok();
        }

        [Route("{surveyid}/questions"), HttpPost]
        public async Task<IHttpActionResult> CreateQuestion(int surveyId, [FromBody] dtoSurveyQuestion question)
        {
            dtoSurveyQuestion dtoQuestion = new dtoSurveyQuestion();
            try {
                question.SurveyId = surveyId;
                dtoQuestion = await _yuyanSvc.CreateSurveyQuestion(question);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(dtoQuestion);
        }

        [Route("{surveyid}/questions"), HttpPut]
        public async Task<IHttpActionResult> UpdateQuestion(int surveyId, [FromBody] dtoSurveyQuestion question)
        {
            dtoSurveyQuestion dtoSurveyQuestion = new dtoSurveyQuestion();

            try {
                question.SurveyId = surveyId;
                dtoSurveyQuestion = await _yuyanSvc.UpdateSurveyQuestion(question);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoSurveyQuestion);
        }

        [Route("{surveyid}/questions/{questionId}"), HttpDelete]
        public async Task<IHttpActionResult> DeleteQuestion(int questionId) {
            try {
                await _yuyanSvc.DeleteSurveyQuestion(questionId);
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
        #endregion

        #region item
        #endregion
    }
}
