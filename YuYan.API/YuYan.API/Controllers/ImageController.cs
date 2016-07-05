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

        [HttpPost]
        [Route("upload")]
        //[ResponseType(typeof(tohow.Models.Image))]
        [AuthenticationFilter(AllowAnonymous = false)]
        public async Task<IHttpActionResult> ImageUpload()
        {
            
            //var principle = HttpContext.Current.User as ToHowAPIUser;
            //var identity = principle.Identity as ToHowAPIIdentity;

            var user = ControllerContext.RequestContext.Principal as YYUser;

            //ImagePostResponse resp = new ImagePostResponse();
            //tohow.Models.Image respModel = new tohow.Models.Image();

            if (!Request.Content.IsMimeMultipartContent())
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));

            try
            {
                var provider = GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                var originalFileName = GetDeserializedFileName(result.FileData.First());
                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);


                string saveToDbSvr = "Save to db";
                /*
                using (tohowEntities db = new Data.tohowEntities())
                {
                    var user = (from u in db.tbProfiles where u.Id == identity.ProfileId.Value && u.IsDeleted == false select u).FirstOrDefault();
                    var imageNameMeta = (from m in db.tbMetaTypes where m.Category == "IMAGE" && m.Name == "Name" select m).FirstOrDefault();
                    var imageExtensionMeta = (from m in db.tbMetaTypes where m.Category == "IMAGE" && m.Name == "Extension" select m).FirstOrDefault();

                    tbImage newImage = new tbImage();
                    newImage.Id = Guid.NewGuid();
                    newImage.CreateDateTime = DateTime.UtcNow;
                    newImage.IsDeleted = false;
                    newImage.IsLocked = false;
                    newImage.IsPublic = true;
                    newImage.IsSold = false;
                    newImage.Points = 0;
                    newImage.tbImageMetas.Add(new tbImageMeta { tbImage = newImage, tbMetaType = imageNameMeta, MetaValue = originalFileName, CreateDateTime = DateTime.UtcNow, tbProfile = user, IsDeleted = false });
                    newImage.tbImageMetas.Add(new tbImageMeta { tbImage = newImage, tbMetaType = imageExtensionMeta, MetaValue = uploadedFileInfo.Extension, CreateDateTime = DateTime.UtcNow, tbProfile = user, IsDeleted = false });
                    newImage.Uri = uploadedFileInfo.Name;
                    newImage.tbProfile = user;
                    db.tbImages.Add(newImage);
                    db.SaveChanges();

                 

                    respModel.Id = newImage.Id;
                    respModel.IsLocked = newImage.IsLocked;
                    respModel.Points = newImage.Points;
                    respModel.CreatedDate = newImage.CreateDateTime;
                    respModel.IsPublic = newImage.IsPublic;
                    
                }*/
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = "~/FileUploads/";
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = fileData.Headers.ContentDisposition.FileName;
            return JsonConvert.DeserializeObject(fileName).ToString();
        }
       
    }


}
