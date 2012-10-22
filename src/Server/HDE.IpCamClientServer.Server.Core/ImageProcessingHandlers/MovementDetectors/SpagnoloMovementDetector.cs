using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    /// <summary>
    /// Built on the article 
    /// "Moving object segmentation by background subtraction and temporal analysis" - P. Spagnolo, T.D’ Orazio *, M. Leo, A. Distante
    /// </summary>
    /// <remarks>Method is not working!</remarks>
    [Obsolete("Method is not working", true)]
    public class SpagnoloMovementDetector : IHandler
    {
        #region Constants

        public const string Article = "\"Moving object segmentation by background subtraction and temporal analysis\" - P. Spagnolo, T.D’ Orazio *, M. Leo, A. Distante";

        public const string RadiometricSimmilarityFrameTFrameTMinus1Motion = "Radiometric Simmilarity (Frame(t), Frame(t-1)): Motion: ";
        public const string RadiometricSimmilarityFrameTFrameTMinus1Stationary = "Radiometric Simmilarity (Frame(t), Frame(t-1)): Stationary: ";
        public const string RadiometricSimmilarityMotionTBackgroundT = "Radiometric Simmilarity (Motion(t), Background(t))";
        public const string DifferenceImage = "DIFF";
        
        public const int DefaultStartingFramesCount = 10; //DEBUG:!!!
        public const int RadiometricDifferenceThrethhold = 2;

        #endregion

        #region Fields

        private readonly IInterceptor _interceptor;
        private int _skipStartingFrames = DefaultStartingFramesCount;

        #endregion

        #region Types

        #endregion

        #region Constructors

        public SpagnoloMovementDetector(IInterceptor interceptor = null)
        {
            _interceptor = interceptor ?? new NullInterceptor();
        }

        #endregion

        #region IHandler Implementation

        public string[] GetDebugWindows()
        {
            return new[]
                {
                    RadiometricSimmilarityFrameTFrameTMinus1Motion,
                    RadiometricSimmilarityFrameTFrameTMinus1Stationary,
                    RadiometricSimmilarityMotionTBackgroundT,  
                    DifferenceImage
                };
        }

        public void Configure(string configurationString)
        {
            //TODO: extract parameters!
        }

        private RadiometricSimilarityImage _previous;
        private Bitmap _grayScaleBackground;
        public string Process(Bitmap bitmap)
        {
            // When video device initializes it adapts to 
            // background for some frames.
            if (_skipStartingFrames > 0)
            {
                _skipStartingFrames--;
                return null;
            }

            using (var grayScaleImage = GrayScaleImageHelper.ToGrayScale(bitmap))
            {
                if (_previous == null)
                {
                    byte[,] imageTMinus1MeanWH;
                    byte[,] imageTMinus1VarianceWH;
                    GrayScaleImageHelper.CalculateMeanAndVarianceM9(
                        grayScaleImage, 
                        out imageTMinus1MeanWH,
                        out imageTMinus1VarianceWH);

                    _previous = new RadiometricSimilarityImage(
                        grayScaleImage,
                        imageTMinus1MeanWH,
                        imageTMinus1VarianceWH
                        );
                    
                    _grayScaleBackground = (Bitmap)grayScaleImage.Clone();
                    return null;
                }

                var bounds = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
                BitmapData bitmapData = grayScaleImage.LockBits(bounds, ImageLockMode.ReadOnly, grayScaleImage.PixelFormat);
                var grayScaleHW = new byte[grayScaleImage.Height * bitmapData.Stride];
                Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayScaleImage.Height * bitmapData.Stride);
                grayScaleImage.UnlockBits(bitmapData);

                byte[,] imageTMean;
                byte[,] imageTVariance;
                byte[,] motionWH;
                bool motionPresents;
                GrayScaleImageHelper.CalculateMeanAndVarianceM9(
                    grayScaleImage.Width,
                    grayScaleImage.Height,
                    bitmapData.Stride,
                    grayScaleHW,

                    out imageTMean,
                    out imageTVariance);
                var current = new RadiometricSimilarityImage(grayScaleImage.Width, 
                    grayScaleImage.Height,
                    bitmapData.Stride,
                    grayScaleHW,
                    imageTMean, imageTVariance);
                
                byte[,] motionData, stationaryData;
                Bitmap motionImage, stationaryImage;
                GrayScaleImageHelper.GetRadiometricSimmilarity(
                    _previous,
                    current,
                    RadiometricDifferenceThrethhold,
                    out motionData,
                    out motionImage,

                    out stationaryData,
                    out stationaryImage);
                {
                    using (motionImage)
                    {
                        _interceptor.Intercept(RadiometricSimmilarityFrameTFrameTMinus1Motion, ImageHelper.ToBytes(motionImage));
                    }

                    using (stationaryImage)
                    {
                        _interceptor.Intercept(RadiometricSimmilarityFrameTFrameTMinus1Stationary, ImageHelper.ToBytes(stationaryImage));
                    }

                    UpdateBackgroundOrForeground(
                        motionData,
                        grayScaleImage.Width, 
                        grayScaleImage.Height,
                        bitmapData.Stride,
                        grayScaleHW,
                        out motionPresents,
                        out motionWH);

                }

                _previous = current;

                using (var image = GrayScaleImageHelper.FromWH(motionWH))
                {
                    _interceptor.Intercept(DifferenceImage, ImageHelper.ToBytes(image));
                }

                if (motionPresents)
                {
                    return "Movement detected!";
                }
            }
            return null;
        }

        private void UpdateBackgroundOrForeground(
            byte[,] rItItm1HW, 
            int width,
            int height,
            int stride,
            byte[] currentFrame,
            //byte[,] background, 
            out bool motionPresents, 
            out byte[,] motionWH)
        {
            motionPresents = false;

//TODO: get access to bytes of tiar
//TODO: get access to bytes of background
//TODO: update background.

            motionWH = new byte[width, height];
            for (int wi = 1; wi < (width-1);wi++)
            {
                for (int hi = 1; hi < (height - 1); hi++)
                {
                    if (rItItm1HW[hi, wi] == 1)
                    {
                        motionWH[wi, hi] = currentFrame[GrayScaleImageHelper.ToDataPosition(wi, hi, stride)];
                        motionPresents = true;
                    }
                    else
                    {
                        //TODO:
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}