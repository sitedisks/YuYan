using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
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
            // try to use Hashids.net to encode !
            dtoSurvey dtoSurvey = new dtoSurvey();

            try
            {
                dtoSurvey = await _yuyanSvc.GetSurveyByURLToken(urltoken);
                if (dtoSurvey == null)
                    return NotFound();

                dtoSurveyShare newShare = new dtoSurveyShare()
                {
                    SurveyId = dtoSurvey.SurveyId,
                    IPAddress = GetClientIp()
                };

                await _yuyanSvc.AddSurveyShare(newShare);
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

        [Route("{urltoken}/title"), HttpGet]
        public async Task<IHttpActionResult> GetSurveyTitleByURLToken(string urltoken) {
            string title = "";

            try {
                title = await _yuyanSvc.GetSurveyTitleByURLToken(urltoken);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(title);
        }

        [Route(""), HttpPost]
        public async Task<IHttpActionResult> SubmitSurvey(dtoSurveyClient surveyClient)
        {
            dtoSurvey dtoSurvey = null;

            try
            {
                surveyClient.IPAddress = GetClientIp();
                dtoSurvey = await _yuyanSvc.SaveSurveyClient(surveyClient);

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

        [Route("{surveyId}/{score}"), HttpGet]
        public async Task<IHttpActionResult> GetSurveyResultByScore(int surveyId, int score) {
            dtoSurveyResult dtoSurveyResult = null;

            try {
                var resultList = await _yuyanSvc.GetSurveyResultsBySurveyId(surveyId, score);
                dtoSurveyResult = resultList.FirstOrDefault();
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(dtoSurveyResult);
        }

        [Route("geoip"), HttpGet]
        public async Task<IHttpActionResult> GetGeoIP(string ip) {
            dtoLocationGeo geoLocation = null;

            try {
                if (ip == null)
                {
                    ip = GetClientIp();
                }
                geoLocation = await _yuyanSvc.GetGeoLocationByIpAddress(ip);
                geoLocation.IpAddress = ip;
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

             return Ok(geoLocation);
        }

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
