using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using YuYan.Domain.DTO;
using YuYan.API.Filter;
using YuYan.Interface.Service;
using System.Net.Http;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Linq;

namespace YuYan.API.Controllers
{
    [RoutePrefix("images")]
    public class ImageController : ApiController
    {
        private readonly IYuYanService _yuyanSvc;

        public ImageController(IYuYanService yuyanSvc)
        {
            _yuyanSvc = yuyanSvc;
        }

        [HttpPost]
        [Route("survey/upload")]
        //[ResponseType(typeof(tohow.Models.Image))]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> ImageUpload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));

            dtoImage theImage = new dtoImage();

            try
            {
                var user = ControllerContext.RequestContext.Principal as YYUser;

                var provider = GetMultipartProvider("Survey");
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                var originalFileName = GetDeserializedFileName(result.FileData.First());
                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

                int id = int.Parse(result.FormData["id"]);

                dtoImage image = new dtoImage
                {
                    ImageType = Domain.Enum.ImageType.SurveyRef,
                    UserId = user.UserId,
                    FileName = originalFileName,
                    RefId = id
                };

                theImage = await _yuyanSvc.InsertImage(image);
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(theImage);
        }

        [HttpGet]
        [Route("{imageId:guid}")]
        [AuthenticationFilter(AllowAnonymous = true)]
        public async Task<IHttpActionResult> ImageRetrieve() {

            try { }
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

        #region private
        private MultipartFormDataStreamProvider GetMultipartProvider(string imageGroup)
        {
            var uploadFolder = "~/FileUploads/" + imageGroup;
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = fileData.Headers.ContentDisposition.FileName;
            return JsonConvert.DeserializeObject(fileName).ToString();
        }
        #endregion
    }


}
