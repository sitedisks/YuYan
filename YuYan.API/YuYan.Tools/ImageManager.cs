using ImageResizer;
using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace YuYan.Tools
{
    public class ImageManager
    {
        public Image ResizeImage(Image source, int? height, int? width, int? quality, bool keepRatio = true)
        {
            Image output = null;

            try
            {
                Instructions settings = new Instructions();

                settings.Scale = ScaleMode.Both;

                if (keepRatio)
                {
                    if (height.HasValue && width.HasValue)
                    {
                        if (height.Value > width.Value) // adjust based on height as it is a larger value
                            settings.Height = height.Value;
                        else // adjust based on width as it is a larger value
                            settings.Width = width.Value;
                    }
                    else
                    {
                        if (height.HasValue)
                            settings.Height = height.Value;
                        if (width.HasValue)
                            settings.Width = width.Value;
                    }
                }
                else
                {
                    if (height.HasValue)
                        settings.Height = height.Value;
                    if (width.HasValue)
                        settings.Width = width.Value;

                    settings.Mode = FitMode.Stretch;
                }

                if (quality.HasValue)
                    settings.JpegQuality = quality.Value;

                ImageJob j = new ImageJob(source.Clone(), typeof(Bitmap), settings);
                ImageBuilder.Current.Build(j);
                output = j.Result as Image;
            }
            catch (Exception ex)
            {
                // logging
                return source;
            }

            return output;
        }

        public Image NotFoundImage()
        {
            var uploadFolder = "~/FileUploads/";
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            var stream = new FileStream(root + "/Not-found.jpg", FileMode.Open, FileAccess.Read);
            return Image.FromStream(stream);
        }

        public Image RetrieveImage(string filename, string filefolder)
        {
            var uploadFolder = "~/FileUploads/" + filefolder;
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            var stream = new FileStream(root + "/" + filename, FileMode.Open, FileAccess.Read);
            return Image.FromStream(stream);
        }
    }
}
