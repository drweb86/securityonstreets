using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    internal static class ImageHelper
    {
        /// <summary>
        /// Calculates mean intensity of pixel.
        /// Variance is calculated as Absolute Deviation
        /// </summary>
        /// <param name="bitmap">Input bitmap</param>
        //public static void CalculateMeanAndVarianceM9ColorBitmap(Bitmap grayScaleBitmap, 
        //    out byte[,] widthHeightDataIntensity,
        //    out byte[,] widthHeightDataVariance)
        //{
        //    widthHeightDataIntensity = new byte[grayScaleBitmap.Height, grayScaleBitmap.Width];
        //    widthHeightDataVariance = new byte[grayScaleBitmap.Height, grayScaleBitmap.Width];

        //    //unsafe
        //    //{
                
        //    //}
        //    //{
        //    //    BitmapData bmd = grayScaleBitmap.LockBits(new Rectangle(0, 0, 10, 10),
        //    //                                              System.Drawing.Imaging.ImageLockMode.ReadOnly, bm.PixelFormat);
        //    //    int PixelSize = 4;

        //    //    for (int y = 0; y < bmd.Height; y++)
        //    //    {

        //    //        byte* row = (byte*) bmd.Scan0 + (y*bmd.Stride);

        //    //        for (int x = 0; x < bmd.Width; x++)
        //    //        {

        //    //            row[x*PixelSize] = 255;

        //    //        }

        //    //    }
        //    //    grayScaleBitmap.UnlockBits(bmd);
        //    //}
            
        //    widthHeightDataIntensity = new byte[grayScaleBitmap.Height, grayScaleBitmap.Width];
        //    widthHeightDataVariance = new byte[grayScaleBitmap.Height, grayScaleBitmap.Width];
        //    for (int widthI = 1; widthI < (grayScaleBitmap.Width - 1); widthI++)
        //    {
        //        for (int heightI = 1; heightI < (grayScaleBitmap.Height - 1); heightI++)
        //        {
        //            var a1 = ToIntensity256(bitmap.GetPixel(widthI - 1, heightI - 1));
        //            var a2 = ToIntensity256(bitmap.GetPixel(widthI - 1, heightI));
        //            var a3 = ToIntensity256(bitmap.GetPixel(widthI - 1, heightI + 1));
        //            var a4 = ToIntensity256(bitmap.GetPixel(widthI, heightI - 1));
        //            var a5 = ToIntensity256(bitmap.GetPixel(widthI, heightI));
        //            var a6 = ToIntensity256(bitmap.GetPixel(widthI, heightI + 1));
        //            var a7 = ToIntensity256(bitmap.GetPixel(widthI + 1, heightI - 1));
        //            var a8 = ToIntensity256(bitmap.GetPixel(widthI + 1, heightI));
        //            var a9 = ToIntensity256(bitmap.GetPixel(widthI + 1, heightI + 1));
        //            var meanIntensity = (byte) ((a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9)/9);
        //            widthHeightDataIntensity[heightI, widthI] = meanIntensity;
        //            widthHeightDataVariance[heightI, widthI] = (byte)((
        //                Math.Abs(a1-meanIntensity) + 
        //                Math.Abs(a2-meanIntensity) + 
        //                Math.Abs(a3-meanIntensity) + 
        //                Math.Abs(a4-meanIntensity) + 
        //                Math.Abs(a5-meanIntensity) + 
        //                Math.Abs(a6-meanIntensity) + 
        //                Math.Abs(a7-meanIntensity) + 
        //                Math.Abs(a8-meanIntensity) + 
        //                Math.Abs(a9-meanIntensity)) / 9);
        //        }
        //    }
        //}

        #region Private Methods

        private static byte ToIntensity256(Color color)
        {
            return (byte)(byte.MaxValue - 1 - color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
        }

        //private static byte ToIntensity256(byte gra)
        //{
        //    return (byte)(byte.MaxValue - 1 - color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
        //}

        #endregion
    }

    /// <summary>
    /// Contains helper functional about gray scale images.
    /// </summary>
    /// <summary>
    /// Grayscale image contains bytes with values ranging from 0 to 255.
    /// </summary>
    public static class GrayScaleImageHelper
    {
        /// <summary>
        /// Calculates mean intensity of pixel.
        /// Variance is calculated as Absolute Deviation
        /// </summary>
        public static void CalculateMeanAndVarianceM9(Bitmap bitmap,
            out byte[,] widthHeightDataIntensity,
            out byte[,] widthHeightDataVariance)
        {
            if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new NotSupportedException("Pixel format is not supported.");
            }

            var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var data = new byte[bitmap.Height * bitmapData.Stride];
            Marshal.Copy(bitmapData.Scan0, data, 0, bitmap.Height * bitmapData.Stride);
            
            widthHeightDataIntensity = new byte[bitmap.Width, bitmap.Height];
            widthHeightDataVariance = new byte[bitmap.Width, bitmap.Height];

            for (int widthI = 1; widthI < (bitmap.Width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (bitmap.Height - 1); heightI++)
                {
                    var a1 = data[ToDataPosition(    widthI - 1, heightI - 1,    bitmapData.Stride)];
                    var a2 = data[ToDataPosition(    widthI - 1, heightI,        bitmapData.Stride)];;
                    var a3 = data[ToDataPosition(    widthI - 1, heightI + 1,    bitmapData.Stride)];
                    var a4 = data[ToDataPosition(    widthI,     heightI - 1,    bitmapData.Stride)];
                    var a5 = data[ToDataPosition(    widthI,     heightI,        bitmapData.Stride)];
                    var a6 = data[ToDataPosition(    widthI,     heightI + 1,    bitmapData.Stride)];
                    var a7 = data[ToDataPosition(    widthI + 1, heightI - 1,    bitmapData.Stride)];
                    var a8 = data[ToDataPosition(    widthI + 1, heightI,        bitmapData.Stride)];
                    var a9 = data[ToDataPosition(    widthI + 1, heightI + 1,    bitmapData.Stride)];
                    
                    var meanIntensity = (byte)((a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9) / 9);
                    widthHeightDataIntensity[widthI, heightI] = meanIntensity;
                    widthHeightDataVariance[widthI, heightI] = (byte)((
                        Math.Abs(a1 - meanIntensity) +
                        Math.Abs(a2 - meanIntensity) +
                        Math.Abs(a3 - meanIntensity) +
                        Math.Abs(a4 - meanIntensity) +
                        Math.Abs(a5 - meanIntensity) +
                        Math.Abs(a6 - meanIntensity) +
                        Math.Abs(a7 - meanIntensity) +
                        Math.Abs(a8 - meanIntensity) +
                        Math.Abs(a9 - meanIntensity)) / 9);
                }
            }

            bitmap.UnlockBits(bitmapData);
        }

        private static int ToDataPosition(int width, int height, int stride)
        {
            return height*stride + width;
        }

        /*public static void CalculateMeanAndVarianceM9(
            Bitmap bitmap1,
            Bitmap bitmap2,
            out long[,] widthHeightDataIntensity)
        {
            widthHeightDataIntensity = new long[bitmap1.Height, bitmap1.Width];

            for (int widthI = 1; widthI < (bitmap1.Width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (bitmap1.Height - 1); heightI++)
                {
                    var a11 = ToIntensity(bitmap1.GetPixel(widthI - 1, heightI - 1));
                    var a12 = ToIntensity(bitmap1.GetPixel(widthI - 1, heightI));
                    var a13 = ToIntensity(bitmap1.GetPixel(widthI - 1, heightI + 1));
                    var a21 = ToIntensity(bitmap1.GetPixel(widthI, heightI - 1));
                    var a22 = ToIntensity(bitmap1.GetPixel(widthI, heightI));
                    var a23 = ToIntensity(bitmap1.GetPixel(widthI, heightI + 1));
                    var a31 = ToIntensity(bitmap1.GetPixel(widthI + 1, heightI - 1));
                    var a32 = ToIntensity(bitmap1.GetPixel(widthI + 1, heightI));
                    var a33 = ToIntensity(bitmap1.GetPixel(widthI + 1, heightI + 1));

                    var b11 = ToIntensity(bitmap2.GetPixel(widthI - 1, heightI - 1));
                    var b12 = ToIntensity(bitmap2.GetPixel(widthI - 1, heightI));
                    var b13 = ToIntensity(bitmap2.GetPixel(widthI - 1, heightI + 1));
                    var b21 = ToIntensity(bitmap2.GetPixel(widthI, heightI - 1));
                    var b22 = ToIntensity(bitmap2.GetPixel(widthI, heightI));
                    var b23 = ToIntensity(bitmap2.GetPixel(widthI, heightI + 1));
                    var b31 = ToIntensity(bitmap2.GetPixel(widthI + 1, heightI - 1));
                    var b32 = ToIntensity(bitmap2.GetPixel(widthI + 1, heightI));
                    var b33 = ToIntensity(bitmap2.GetPixel(widthI + 1, heightI + 1));

                    var c11 = a11 * b11 + a12 * b21 + a13 * b31;
                    var c12 = a11 * b12 + a12 * b22 + a13 * b32;
                    var c13 = a11 * b13 + a12 * b23 + a13 * b33;

                    var c21 = a21 * b11 + a22 * b21 + a23 * b31;
                    var c22 = a21 * b12 + a22 * b22 + a23 * b32;
                    var c23 = a21 * b13 + a22 * b23 + a23 * b33;

                    var c31 = a31 * b11 + a32 * b21 + a33 * b31;
                    var c32 = a31 * b12 + a32 * b22 + a33 * b32;
                    var c33 = a31 * b13 + a32 * b23 + a33 * b33;

                    widthHeightDataIntensity[heightI, widthI] = (c11 + c12 + c13 + c21 + c22 + c23 + c31 + c32 + c33) / 9;
                }
            }
        }*/
        
        public static Bitmap BeginImage(int width, int height, out byte[,] dataHeightWidth, out GCHandle handle)
        {
            int stride = 4 * ((width * 8 + 31) / 32);
            dataHeightWidth = new byte[height, stride];
            handle = GCHandle.Alloc(dataHeightWidth, GCHandleType.Pinned);

            var result = new Bitmap(width, height, stride, PixelFormat.Format8bppIndexed, handle.AddrOfPinnedObject());
            var palette = result.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            result.Palette = palette;
            return result;
        }

        public static void EndImage(GCHandle handle)
        {
            handle.Free();
        }

        /// <summary>
        /// Converts a bitmap into an 8-bit grayscale bitmap
        /// </summary>
        public static Bitmap ToGrayScale(Bitmap coloredImage)
        {
            int width = coloredImage.Width,
                height = coloredImage.Height,
                r, ic, oc, stride, outputStride, bytesPerPixel;
            var inputFormat = coloredImage.PixelFormat;

            //Create the new bitmap
            Bitmap output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //Build a grayscale color Palette
            ColorPalette palette = output.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            output.Palette = palette;

            //No need to convert formats if already in 8 bit
            if (inputFormat == PixelFormat.Format8bppIndexed)
            {
                output = (Bitmap)coloredImage.Clone();

                //Make sure the palette is a grayscale palette and not some other
                //8-bit indexed palette
                output.Palette = palette;

                return output;
            }

            //Get the number of bytes per pixel
            switch (inputFormat)
            {
                case PixelFormat.Format24bppRgb: bytesPerPixel = 3; break;
                case PixelFormat.Format32bppArgb: bytesPerPixel = 4; break;
                case PixelFormat.Format32bppRgb: bytesPerPixel = 4; break;
                default: throw new InvalidOperationException("Image format not supported");
            }

            //Lock the images
            BitmapData bmpData = coloredImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                                                       inputFormat);
            BitmapData outputData = output.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                                                    PixelFormat.Format8bppIndexed);
            stride = bmpData.Stride;
            outputStride = outputData.Stride;

            //Traverse each pixel of the image
            unsafe
            {
                byte* bmpPtr = (byte*)bmpData.Scan0.ToPointer(),
                outputPtr = (byte*)outputData.Scan0.ToPointer();

                if (bytesPerPixel == 3)
                {
                    //Convert the pixel to it's luminance using the formula:
                    // L = .299*R + .587*G + .114*B
                    //Note that ic is the input column and oc is the output column
                    for (r = 0; r < height; r++)
                        for (ic = oc = 0; oc < width; ic += 3, ++oc)
                            outputPtr[r * outputStride + oc] = (byte)(int)
                                (0.299f * bmpPtr[r * stride + ic] +
                                 0.587f * bmpPtr[r * stride + ic + 1] +
                                 0.114f * bmpPtr[r * stride + ic + 2]);
                }
                else //bytesPerPixel == 4
                {
                    //Convert the pixel to it's luminance using the formula:
                    // L = alpha * (.299*R + .587*G + .114*B)
                    //Note that ic is the input column and oc is the output column
                    for (r = 0; r < height; r++)
                        for (ic = oc = 0; oc < width; ic += 4, ++oc)
                            outputPtr[r * outputStride + oc] = (byte)(int)
                                ((bmpPtr[r * stride + ic] / 255.0f) *
                                (0.299f * bmpPtr[r * stride + ic + 1] +
                                 0.587f * bmpPtr[r * stride + ic + 2] +
                                 0.114f * bmpPtr[r * stride + ic + 3]));
                }
            }

            //Unlock the images
            coloredImage.UnlockBits(bmpData);
            output.UnlockBits(outputData);

            return output;
        }

        public static Bitmap ToGrayScale(string dataFile)
        {
            return ToGrayScale(ArrayHelper.FromFile(dataFile));
        }

        public static Bitmap ToGrayScale(byte[,] dataWidthHeight)
        {
            var width = dataWidthHeight.GetLength(0);
            var height = dataWidthHeight.GetLength(1);
            byte[,] dataHeightWidth;
            GCHandle handle;
            var result = BeginImage(
                dataWidthHeight.GetLength(0),
                dataWidthHeight.GetLength(1),

                out dataHeightWidth,
                out handle);

            for (int widthI = 0; widthI < width; widthI++)
            {
                for (int heightI = 0; heightI < height; heightI++)
                {
                    dataHeightWidth[heightI, widthI] = dataWidthHeight[widthI, heightI];
                }
            }
            EndImage(handle);
            return result;
        }
    }

    public static class ArrayHelper
    {
        public static bool Compare(byte[,] a, byte[,] b, int thethhold)
        {
            if (a.GetLength(0) != b.GetLength(0) ||
                a.GetLength(1) != b.GetLength(1))
            {
                return false;
            }

            for (int dim1 = 0; dim1 < a.GetLength(0); dim1++)
                for (int dim2 = 0; dim2 < a.GetLength(1); dim2++)
                {
                    if (Math.Abs(a[dim1, dim2] - b[dim1, dim2]) > thethhold)
                    {
                        return false;
                    }
                }
            return true;
        }

        public static byte[,] FromFile(string file)
        {
            var data = File.ReadAllLines(file);
            var width = data[0].Split('\t').Length;
            byte[,] dataWidthHeight = new byte[width, data.Length];
            for (int heightI = 0; heightI < data.Length; heightI++)
            {
                var splited = data[heightI].Split('\t');
                for (int widthI = 0; widthI < width; widthI++)
                {
                    dataWidthHeight[widthI, heightI] = byte.Parse(splited[widthI]);
                }
            }
            return dataWidthHeight;
        }
    }
}