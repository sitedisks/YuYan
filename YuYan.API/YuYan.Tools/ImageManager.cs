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
                //ResizeSettings settings = new ResizeSettings();
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


                //ImageBuilder.Current.Build(source.Clone(), output, settings);
                // http://imageresizing.net/docs/v4/managed
                //Image b = ImageBuilder.Current.Build(source.Clone(), settings);

                ImageBuilder.Current.Build(new ImageJob(source.Clone(), output, settings));
                
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

        public Image RetrieveImage(string filename)
        {
            var uploadFolder = "~/FileUploads/";
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            var stream = new FileStream(root + "/" + filename, FileMode.Open, FileAccess.Read);
            return Image.FromStream(stream);
        }
    }
}
