using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YuYan.Domain.DTO;
using YuYan.API.Filter;
using YuYan.Interface.Service;

namespace YuYan.API.Controllers
{
    [RoutePrefix("surveys")]
    public class SurveyController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public SurveyController(IYuYanService yuyanSvc)
        {
            _yuyanSvc = yuyanSvc;
        }

        #region survey
        // GET /surveys/2
        [Route("{surveyid}"), HttpGet]  
        public async Task<IHttpActionResult> GetSurveyBySurveyId(int surveyId)
        {
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
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> CreateSurvey([FromBody] dtoSurvey survey)
        {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try
            {
                var user = ControllerContext.RequestContext.Principal as YYUser;
                if (user != null)
                    survey.UserId = user.UserId;
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

        [Route("{surveyid}"), HttpPut]
        public async Task<IHttpActionResult> UpdateSurvey(int surveyId, [FromBody] dtoSurvey survey)
        {
            dtoSurvey dtoSurvey = new dtoSurvey();

            try
            {
                survey.SurveyId = surveyId;
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
        public async Task<IHttpActionResult> DeleteSurvey(int surveyId)
        {
            try
            {
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
        public async Task<IHttpActionResult> GetQuestionBySurveyId(int surveyId)
        {
            IList<dtoSurveyQuestion> questionList = new List<dtoSurveyQuestion>();

            try
            {
                questionList = await _yuyanSvc.GetSurveyQuestionsBySurveyId(surveyId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(questionList);
        }

        [Route("{surveyid}/questions/{questionid}"), HttpGet]
        public async Task<IHttpActionResult> GetQuestionByQuestionId(int surveyId, int questionId)
        {
            dtoSurveyQuestion question = new dtoSurveyQuestion();

            try
            {
                question = await _yuyanSvc.GetSurveyQuestionByQuestionId(questionId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(question);
        }

        [Route("{surveyid}/questions"), HttpPost]
        public async Task<IHttpActionResult> CreateQuestion(int surveyId, [FromBody] dtoSurveyQuestion question)
        {
            dtoSurveyQuestion dtoQuestion = new dtoSurveyQuestion();
            try
            {
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

        [Route("{surveyid}/questions/{questionid}"), HttpPut]
        public async Task<IHttpActionResult> UpdateQuestion(int surveyId, int questionId, [FromBody] dtoSurveyQuestion question)
        {
            dtoSurveyQuestion dtoSurveyQuestion = new dtoSurveyQuestion();

            try
            {
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

        [Route("{surveyid}/questions/{questionid}"), HttpDelete]
        public async Task<IHttpActionResult> DeleteQuestion(int surveyId, int questionId)
        {
            try
            {
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
        [Route("{surveyid}/questions/{questionid}/items"), HttpGet]
        public async Task<IHttpActionResult> GetItemsByQuestionId(int surveyid, int questionId)
        {
            IList<dtoSurveyQuestionItem> itemList = new List<dtoSurveyQuestionItem>();

            try
            {
                itemList = await _yuyanSvc.GetQuestionItemsByQuestionId(questionId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(itemList);
        }

        [Route("{surveyid}/questions/{questionid}/items/{itemid}"), HttpGet]
        public async Task<IHttpActionResult> GetItemByItemId(int surveyid, int questionid, int itemId)
        {
            dtoSurveyQuestionItem item = new dtoSurveyQuestionItem();

            try
            {
                item = await _yuyanSvc.GetQuestionItemByItemId(itemId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(item);
        }

        [Route("{surveyid}/questions/{questionid}/items"), HttpPost]
        public async Task<IHttpActionResult> CreateItem(int surveyId, int questionId, [FromBody] dtoSurveyQuestionItem item)
        {
            dtoSurveyQuestionItem dtoItem = new dtoSurveyQuestionItem();

            try
            {
                item.QuestionId = questionId;
                dtoItem = await _yuyanSvc.CreateSurveyQuestionItem(item);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoItem);
        }

        [Route("{surveyid}/questions/{questionid}/items/{itemid}"), HttpPut]
        public async Task<IHttpActionResult> UpdateItem(int surveyId, int questionId, int itemId, [FromBody] dtoSurveyQuestionItem item)
        {
            dtoSurveyQuestionItem dtoItem = new dtoSurveyQuestionItem();

            try
            {
                item.QuestionId = questionId;
                dtoItem = await _yuyanSvc.UpdateSurveyQuestionItem(item);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoItem);
        }

        [Route("{surveyid}/questions/{questionid}/items/{itemid}"), HttpDelete]
        public async Task<IHttpActionResult> DeleteItem(int surveyId, int questionId, int itemId)
        {
            try
            {
                await _yuyanSvc.DeleteSurveyQuestionItem(itemId);
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
    }
}
