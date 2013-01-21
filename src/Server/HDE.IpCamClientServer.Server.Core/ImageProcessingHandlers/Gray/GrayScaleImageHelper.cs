using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray
{
    /// <summary>
    /// Contains helper functional about gray scale images.
    /// </summary>
    /// <summary>
    /// Grayscale image contains bytes with values ranging from 0 to 255.
    /// </summary>
    public static class GrayScaleImageHelper
    {
        public static void Invert(byte[] dataHW, int stride, int width, int height)
        {
            for (int widthI = 0; widthI < width; widthI++)
            {
                for (int heightI = 0; heightI < height; heightI++)
                {
                    var position = ToDataPosition(widthI, heightI, stride);
                    dataHW[position] = (byte)(byte.MaxValue - dataHW[position]);
                }
            }
        }

        public static void ApplySaltAndPapperNoise(int percents, byte[] dataHW, int stride, int width, int height)
        {
            var rand = new Random();
            for (int i = 0; i < (double)(width * height) * percents / 100.0; i++)
            {
                dataHW[ToDataPosition(rand.Next(width), rand.Next(height), stride)] = (byte)rand.Next(2);
            }
        }
        
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

            CalculateMeanAndVarianceM9(bitmap.Width, bitmap.Height, bitmapData.Stride,
                                       data,
                                       out widthHeightDataIntensity,
                                       out widthHeightDataVariance);

            bitmap.UnlockBits(bitmapData);
        }

        /// <summary>
        /// Calculates mean intensity of pixel.
        /// Variance is calculated as Absolute Deviation
        /// </summary>
        public static void CalculateMeanAndVarianceM9(
            int width,
            int height,
            int stride,
            byte[] data,
            out byte[,] widthHeightDataIntensity,
            out byte[,] widthHeightDataVariance)
        {
            widthHeightDataIntensity = new byte[width, height];
            widthHeightDataVariance = new byte[width, height];

            for (int widthI = 1; widthI < (width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (height - 1); heightI++)
                {
                    var a1 = data[ToDataPosition(widthI - 1, heightI - 1, stride)];
                    var a2 = data[ToDataPosition(widthI - 1, heightI, stride)]; ;
                    var a3 = data[ToDataPosition(widthI - 1, heightI + 1, stride)];
                    var a4 = data[ToDataPosition(widthI, heightI - 1, stride)];
                    var a5 = data[ToDataPosition(widthI, heightI, stride)];
                    var a6 = data[ToDataPosition(widthI, heightI + 1, stride)];
                    var a7 = data[ToDataPosition(widthI + 1, heightI - 1, stride)];
                    var a8 = data[ToDataPosition(widthI + 1, heightI, stride)];
                    var a9 = data[ToDataPosition(widthI + 1, heightI + 1, stride)];

                    var meanIntensity = (a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9) / 9.0;
                    widthHeightDataIntensity[widthI, heightI] = (byte)Math.Round(meanIntensity);
                    widthHeightDataVariance[widthI, heightI] = (byte)Math.Round((
                                                                                    Math.Abs(a1 - meanIntensity) +
                                                                                    Math.Abs(a2 - meanIntensity) +
                                                                                    Math.Abs(a3 - meanIntensity) +
                                                                                    Math.Abs(a4 - meanIntensity) +
                                                                                    Math.Abs(a5 - meanIntensity) +
                                                                                    Math.Abs(a6 - meanIntensity) +
                                                                                    Math.Abs(a7 - meanIntensity) +
                                                                                    Math.Abs(a8 - meanIntensity) +
                                                                                    Math.Abs(a9 - meanIntensity)) / 9.0);
                }
            }
        }

        public static int ToDataPosition(int width, int height, int stride)
        {
            return height*stride + width;
        }
        
        // this does not work.
        public static void GetRadiometricSimmilarity(
            RadiometricSimilarityImage image1,
            RadiometricSimilarityImage image2,

            int threthhold,

            out byte[,] motionData,
            out Bitmap motionImage,

            out byte[,] stationaryData,
            out Bitmap stationaryImage

            )
        {
            // Radiometric simillarity.
            GCHandle handleMotion;
            GCHandle handleStationary;
            motionImage = BeginImage(image1.Width, image1.Height, out motionData, out handleMotion);
            stationaryImage = BeginImage(image1.Width, image1.Height, out stationaryData, out handleStationary);
            int[,] firstItemWH;
            GetMultiplier1(image1, image2, out firstItemWH);
            for (int widthI = 1; widthI < (image1.Width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (image1.Height - 1); heightI++)
                {
                    var resultHW = (
                            (firstItemWH[widthI, heightI] - image1.MeanWH[widthI, heightI] * image2.MeanWH[widthI, heightI])
                            /
                            Math.Sqrt(image1.VarianceWH[widthI, heightI] * image2.VarianceWH[widthI, heightI]));
                    //TODO: гистограмма результатов.
                    if (resultHW < 400000)
                    {
                        motionData[heightI, widthI] = byte.MaxValue;
                        stationaryData[heightI, widthI] = 0;
                    }
                    else
                    {
                        motionData[heightI, widthI] = 0;
                        stationaryData[heightI, widthI] = byte.MaxValue;
                    }
                }
            }

            EndImage(handleMotion);
            EndImage(handleStationary);
        }
        
        private static void GetMultiplier1(
            RadiometricSimilarityImage image1,
            RadiometricSimilarityImage image2,
            out int[,] widthHeightDataIntensity)
        {
            widthHeightDataIntensity = new int[image1.Width, image1.Height];
            var data1 = image1.ImageData;
            var data2 = image2.ImageData;
            var width = image1.Width;
            var height = image1.Height;
            var stride = image1.Stride;
            for (int widthI = 1; widthI < (width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (height - 1); heightI++)
                {
                    var a11 = data1[ToDataPosition(    widthI - 1, heightI - 1,    stride)];
                    var a12 = data1[ToDataPosition(    widthI - 1, heightI,        stride)];;
                    var a13 = data1[ToDataPosition(    widthI - 1, heightI + 1,    stride)];
                    var a21 = data1[ToDataPosition(    widthI,     heightI - 1,    stride)];
                    var a22 = data1[ToDataPosition(    widthI,     heightI,        stride)];
                    var a23 = data1[ToDataPosition(    widthI,     heightI + 1,    stride)];
                    var a31 = data1[ToDataPosition(    widthI + 1, heightI - 1,    stride)];
                    var a32 = data1[ToDataPosition(    widthI + 1, heightI,        stride)];
                    var a33 = data1[ToDataPosition(    widthI + 1, heightI + 1,    stride)];

                    var b11 = data2[ToDataPosition(     widthI - 1, heightI - 1,   stride)];
                    var b12 = data2[ToDataPosition(     widthI - 1, heightI,       stride)];
                    var b13 = data2[ToDataPosition(     widthI - 1, heightI + 1,   stride)];
                    var b21 = data2[ToDataPosition(     widthI,     heightI - 1,   stride)];
                    var b22 = data2[ToDataPosition(     widthI,     heightI,       stride)];
                    var b23 = data2[ToDataPosition(     widthI,     heightI + 1,   stride)];
                    var b31 = data2[ToDataPosition(     widthI + 1, heightI - 1,   stride)];
                    var b32 = data2[ToDataPosition(     widthI + 1, heightI,       stride)];
                    var b33 = data2[ToDataPosition(     widthI + 1, heightI + 1,   stride)];

                    var c11 = a11 * b11 + a12 * b21 + a13 * b31;
                    var c12 = a11 * b12 + a12 * b22 + a13 * b32;
                    var c13 = a11 * b13 + a12 * b23 + a13 * b33;

                    var c21 = a21 * b11 + a22 * b21 + a23 * b31;
                    var c22 = a21 * b12 + a22 * b22 + a23 * b32;
                    var c23 = a21 * b13 + a22 * b23 + a23 * b33;

                    var c31 = a31 * b11 + a32 * b21 + a33 * b31;
                    var c32 = a31 * b12 + a32 * b22 + a33 * b32;
                    var c33 = a31 * b13 + a32 * b23 + a33 * b33;

                    
                    var result = (int)Math.Round(((c11 + c12 + c13 + c21 + c22 + c23 + c31 + c32 + c33) / 9.0)*2.0/3.0);
                    if (result > byte.MaxValue*byte.MaxValue*2)
                    {
                        throw new InvalidDataException();
                    }

                    widthHeightDataIntensity[widthI, heightI] = result;
                }
            }
        }
        
        public static Bitmap FromWH(byte[,] dataWH)
        {
            byte[,] dataHW;
            GCHandle handle;
            int width = dataWH.GetLength(0);
            int height = dataWH.GetLength(1);
            var result = BeginImage(width,
                                    height,
                                    out dataHW,
                                    out handle);

            for (int wi = 0; wi < width; wi++)
            {
                for (int hi = 0; hi < height; hi++)
                {
                    dataHW[hi, wi] = dataWH[wi, hi];
                }
            }

            EndImage(handle);

            return result;
        }

        [Obsolete("Memory leaks")]
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

        [Obsolete("Memory leaks")]
        public static void EndImage(GCHandle handle)
        {
//            Marshal.FreeCoTaskMem(handle.AddrOfPinnedObject());
            handle.Free();
        }

        public static void BeginImage2(int width, int height, out byte[,] dataHeightWidth, out 
            Tuple<Bitmap, GCHandle> token)
        {
            int stride = 4 * ((width * 8 + 31) / 32);
            dataHeightWidth = new byte[height, stride];
            var handle = GCHandle.Alloc(dataHeightWidth, GCHandleType.Pinned);

            var result = new Bitmap(width, height, stride, PixelFormat.Format8bppIndexed, handle.AddrOfPinnedObject());
            var palette = result.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            result.Palette = palette;
            token = new Tuple<Bitmap, GCHandle>(result, handle);
        }

        public static byte[] EndImage2(Tuple<Bitmap, GCHandle> token)
        {
            var result = ImageHelper.ToBytes(token.Item1);

            token.Item2.Free();
            token.Item1.Dispose();

            return result;
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
                width,
                height,

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

        [Obsolete("Memory leaks!")]
        public static Bitmap FromData(int width, int height, int stride, byte[] data)
        {
            byte[,] dataHeightWidth;
            GCHandle handle;
            var result = BeginImage(width, height, out dataHeightWidth, out handle);
            for (int widthI = 0; widthI < width; widthI++)
            {
                for (int heightI = 0; heightI < height; heightI++)
                {
                    dataHeightWidth[heightI, widthI] = data[ToDataPosition(widthI, heightI, stride)];
                }
            }
            EndImage(handle);
            return result;
        }

        public static byte[] FromData2(int width, int height, int stride, byte[] data)
        {
            byte[,] dataHeightWidth;
            Tuple<Bitmap, GCHandle> token;
            BeginImage2(width, height, out dataHeightWidth, out token);
            for (int widthI = 0; widthI < width; widthI++)
            {
                for (int heightI = 0; heightI < height; heightI++)
                {
                    dataHeightWidth[heightI, widthI] = data[ToDataPosition(widthI, heightI, stride)];
                }
            }
            return EndImage2(token);
        }
    }
}