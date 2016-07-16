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
        [Route(""), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetSurveysByOwner(int? page = null, int? row = null, bool? actived = null)
        {
            IList<dtoSurvey> surveyList = new List<dtoSurvey>();

            try
            {
                var user = ControllerContext.RequestContext.Principal as YYUser;
                surveyList = await _yuyanSvc.GetSurveysByUserId(user.UserId, page, row, actived);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(surveyList);
        }

        [Route("count"), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetSurveyTotalCountByOwner()
        {

            var countObj = new { SurveyCount = 0 };
            try
            {
                var user = ControllerContext.RequestContext.Principal as YYUser;
                int count = await _yuyanSvc.GetTotalSurveyCountByUserId(user.UserId);
                countObj = new { SurveyCount = count };
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(countObj);
        }

        // Survey CRUD - Get
        [Route("{surveyid}"), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
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

        // Survey CRUD - Create
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
                if (dtoSurvey.SurveyId > 0) {
                    // create default result 
                    dtoSurveyResult firstSurveyResult = new dtoSurveyResult { 
                        MinScore = 0,
                        MaxScore = 100,
                        SurveyId = dtoSurvey.SurveyId,
                        Title = "Thank you!",
                        Description = "We would like to thank you for your participation and attendance at "
                        + "our survey at CHORICE. Your presence together with your active contributions, "
                        + "feedback and ideas was greatly appreciated and has gone towards making us a great success.",
                        ShowStatistics = false // default not show the statistic
                    };
                    var dtoResult = await _yuyanSvc.CreateSurveyResult(firstSurveyResult);
                }
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

        // Survey CRUD - Update
        [Route("{surveyid}"), HttpPut]
        [AuthenticationFilter(AllowAnonymous = false)]
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

        // Survey CRUD - remove / delete
        [Route("{surveyid}"), HttpDelete]
        [AuthenticationFilter(AllowAnonymous = false)]
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
        #endregion

        #region question
        [Route("{surveyid}/questions"), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetQuestionsBySurveyId(int surveyId)
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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
        [AuthenticationFilter(AllowAnonymous = false)]
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

        #region result
        [Route("{surveyid}/results"), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetSurveyResultsBySurveyId(int surveyId)
        {
            IList<dtoSurveyResult> resultList = new List<dtoSurveyResult>();

            try
            {
                resultList = await _yuyanSvc.GetSurveyResultsBySurveyId(surveyId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(resultList);
        }

        [Route("{surveyId}/results/{resultId}"), HttpGet]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> GetSurveyResultByResultId(int surveyId, int resultId)
        {
            dtoSurveyResult result = new dtoSurveyResult();

            try
            {
                result = await _yuyanSvc.GetSurveyResultByResultId(resultId);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(result);
        }

        [Route("{surveyId}/results"), HttpPost]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> CreateSurveyResult(int surveyId, [FromBody] dtoSurveyResult result)
        {
            dtoSurveyResult dtoResult = new dtoSurveyResult();
            try
            {
                result.SurveyId = surveyId;
                dtoResult = await _yuyanSvc.CreateSurveyResult(result);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(dtoResult);
        }

        [Route("{surveyid}/results/{resultId}"), HttpPut]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> UpdateResult(int surveyId, int resultId, [FromBody] dtoSurveyResult result)
        {
            dtoSurveyResult dtoResult = new dtoSurveyResult();

            try
            {
                result.SurveyId = surveyId;
                dtoResult = await _yuyanSvc.UpdateSurveyResult(result);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoResult);
        }

        [Route("{surveyid}/results/{resultId}"), HttpDelete]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> DeleteResult(int surveyId, int resultId)
        {
            try
            {
                await _yuyanSvc.DeleteSurveyResult(resultId);
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
