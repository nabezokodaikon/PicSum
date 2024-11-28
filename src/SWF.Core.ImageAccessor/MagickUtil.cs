using ImageMagick;
using System.Runtime.Versioning;

namespace SWF.Core.ImageAccessor
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal static class MagickUtil
    {
        public static Bitmap ReadImageFile(Stream stream)
        {
            using (var image = new MagickImage(stream))
            {
                return image.ToBitmap();
            }
        }

        public static void SaveFile(
            string srcFilePath, string destFilePath, MagickFormat format, uint quality)
        {
            using (var srcImage = new MagickImage(srcFilePath))
            {
                srcImage.Format = format;
                srcImage.Quality = quality;
                srcImage.Write(destFilePath);
            }
        }
    }
}
