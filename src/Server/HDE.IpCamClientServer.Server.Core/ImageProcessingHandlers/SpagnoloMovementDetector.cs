using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    /// <summary>
    /// Built on the article 
    /// "Moving object segmentation by background subtraction and temporal analysis" - P. Spagnolo, T.D’ Orazio *, M. Leo, A. Distante
    /// </summary>
//TODO: system dhould create detector upon settings!
    public class SpagnoloMovementDetector : IHandler
    {
        #region Constants

        public const string RadiometricSimmilarity = "Radiometric Simmilarity";
        public const string DifferenceImage = "DIFF";
        
        public const int DefaultStartingFramesCount = 1; //DEBUG:!!!
        public const double RadiometricDifferenceThrethhold = 50;

        #endregion

        #region Fields

        private readonly IInterceptor _interceptor;
        private int _skipStartingFrames = DefaultStartingFramesCount;

        #endregion

        #region Constructors

        public SpagnoloMovementDetector(IInterceptor interceptor = null)
        {
            _interceptor = interceptor ?? new NullInterceptor();
        }

        #endregion

        #region IHandler Implementation

        public void Configure(string configurationString)
        {
            //TODO: extract parameters!
        }

        private int width_;
        private int height_;
        private int stride_;

        private Bitmap _grayScaleFrameTMinus1;
        private byte[,] _imageTMinus1MeanWH;
        private byte[,] _imageTMinus1VarianceWH;
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
                if (_grayScaleFrameTMinus1 == null ||
                    _grayScaleBackground == null)
                {
                    _grayScaleFrameTMinus1 = (Bitmap)grayScaleImage.Clone();
                    GrayScaleImageHelper.CalculateMeanAndVarianceM9(
                        _grayScaleFrameTMinus1, 
                        out _imageTMinus1MeanWH,
                        out _imageTMinus1VarianceWH);
                    _grayScaleBackground = (Bitmap)grayScaleImage.Clone();
                    return null;
                }

                var bounds = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
                BitmapData bitmapData = grayScaleImage.LockBits(bounds, ImageLockMode.ReadOnly, grayScaleImage.PixelFormat);
                var grayScaleHW = new byte[grayScaleImage.Height * bitmapData.Stride];
                Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayScaleImage.Height * bitmapData.Stride);

                byte[,] imageTMean;
                byte[,] imageTVariance;
                byte[,] temporalImageAnalysisResultHW;
                byte[,] motionWH;
                bool motionPresents;
                using (var temporalImageAnalysisResult = GetRadiometricSimmilarity(
                    grayScaleImage.Width, 
                    grayScaleImage.Height,
                    bitmapData.Stride,
                    grayScaleHW,
                    out temporalImageAnalysisResultHW, 
                    out imageTMean,
                    out imageTVariance))
                {
                    _interceptor.Intercept(RadiometricSimmilarity, temporalImageAnalysisResult);

                    UpdateBackgroundOrForeground(
                        temporalImageAnalysisResultHW,
                        grayScaleImage.Width, 
                        grayScaleImage.Height,
                        bitmapData.Stride,
                        grayScaleHW,
                        out motionPresents,
                        out motionWH);

                }

                _imageTMinus1MeanWH = imageTMean;
                _imageTMinus1VarianceWH = imageTVariance;
                _grayScaleFrameTMinus1 = (Bitmap)grayScaleImage.Clone();
                grayScaleImage.UnlockBits(bitmapData);

                _interceptor.Intercept(DifferenceImage, GrayScaleImageHelper.FromWH(motionWH));

                if (motionPresents)
                {
                    return "Movement detected!";
                }
            }
            return null;
        }

        private void UpdateBackgroundOrForeground(
            byte[,] radimetricAnalysisResultHW, 
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
                    if (radimetricAnalysisResultHW[hi, wi] > RadiometricDifferenceThrethhold)
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

        // Radometric Simillarity returns doubles 0..1.
        // They are converted to grayscale bitmap for quantification and future analysis.
        private Bitmap GetRadiometricSimmilarity(
            int width,
            int height,
            int stride,
            byte[] grayScaleData,
            out byte[,] dataHeigthWidth, 
            out byte[,] imageTMeanWH, 
            out byte[,] imageTVarianceWH)
        {
            GrayScaleImageHelper.CalculateMeanAndVarianceM9(
                width,
                height,
                stride,
                grayScaleData, 
                
                out imageTMeanWH, 
                out imageTVarianceWH);

            // Radiometric simillarity.
            GCHandle handle;
            var result = GrayScaleImageHelper.BeginImage(width, height, out dataHeigthWidth, out handle);
            double[,] firstItemWH;
            GrayScaleImageHelper.GetMultiplier1(width, height, stride, grayScaleData, _grayScaleFrameTMinus1, out firstItemWH);
            for (int widthI = 1; widthI < (width - 1); widthI++)
            {
                for (int heightI = 1; heightI < (height - 1); heightI++)
                {
                    dataHeigthWidth[heightI, widthI] = (byte)
                        (
                            byte.MaxValue * (   firstItemWH[widthI, heightI] -
                                imageTMeanWH[widthI, heightI] * _imageTMinus1MeanWH[widthI, heightI]
                            ) 
                            /
                            Math.Sqrt(imageTVarianceWH[widthI, heightI] * _imageTMinus1VarianceWH[widthI, heightI]));
                }
            }

            GrayScaleImageHelper.EndImage(handle);
            return result;
        }

        #endregion
    }
}