using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace YuYan.Tools
{
    public static class ImageExtensions
    {
        public static Image Resize(this Image source, int? height, int? width, int? quality, bool keepRatio = true)
        {
            try
            {
                ImageManager im = new ImageManager();
                return im.ResizeImage(source, height, width, quality, keepRatio);
            }
            catch (Exception ex)
            {
                // log exception
            }

            return source;
        }

        public static Stream ToStream(this Image source)
        {
            var stream = new System.IO.MemoryStream();
            source.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }
    }
}
