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
using YuYan.Tools;

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
        [Route("upload/survey")]
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

                int refId = int.Parse(result.FormData["refId"]);
                int typeId = int.Parse(result.FormData["typeId"]);

                dtoImage image = new dtoImage
                {
                    ImageType = (Domain.Enum.ImageType)typeId,
                    UserId = user.UserId,
                    FileName = originalFileName,
                    Uri = uploadedFileInfo.Name,
                    RefId = refId
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
        public async Task<IHttpActionResult> ImageRetrieve([FromUri] Guid imageId, int width = 0)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            ImageManager im = new ImageManager();
            System.Drawing.Image image = null;

            try
            {
                var img = await _yuyanSvc.GetImage(imageId);

                if (img == null)
                    image = im.NotFoundImage();
                else
                    image = im.RetrieveImage(img.Uri);

                response.StatusCode = HttpStatusCode.OK;

                if (width != 0)
                    response.Content = new StreamContent(image.Resize(null, width, null).ToStream());
                else
                    response.Content = new StreamContent(image.ToStream());

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
            }
            catch (ApplicationException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return ResponseMessage(response);
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
