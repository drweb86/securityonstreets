using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray
{
    public class RadiometricSimilarityImage
    {
        #region Fields

        internal readonly byte[] ImageData;
        internal readonly byte[,] MeanWH;
        internal readonly byte[,] VarianceWH;

        internal readonly int Width;
        internal readonly int Height;
        internal readonly int Stride;

        #endregion

        #region Constructors

        public RadiometricSimilarityImage(
            Bitmap image,
            byte[,] meanWH,
            byte[,] varianceWH)
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new NotSupportedException("Pixel format is not supported.");
            }

            Width = image.Width;
            Height = image.Height;
            var bounds = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData bitmapData = image.LockBits(bounds, ImageLockMode.ReadOnly, image.PixelFormat);
            ImageData = new byte[image.Height * bitmapData.Stride];
            Stride = bitmapData.Stride;
            Marshal.Copy(bitmapData.Scan0, ImageData, 0, image.Height * bitmapData.Stride);
            image.UnlockBits(bitmapData);

            MeanWH = meanWH;
            VarianceWH = varianceWH;
        }

        public RadiometricSimilarityImage(
            int width,
            int height,
            int stride,
            byte[] imageData,
            byte[,] meanWH,
            byte[,] varianceWH)
        {
            Width = width;
            Height = height;
            Stride = stride;
            ImageData = imageData;
            MeanWH = meanWH;
            VarianceWH = varianceWH;
        }

        #endregion
    }
}