using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{

    public static class ImageHelper
    {
        public static byte[] ToBytes(Image imageIn)
        {
            using (var stream = new MemoryStream())
            {
                imageIn.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        public static Image FromBytes(byte[] byteArrayIn)
        {
            return Image.FromStream(new MemoryStream(byteArrayIn));
        }
    }
}