using System;
using System.Drawing;
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
        public const double RadiometricDifferenceThrethhold = 0.4;

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

        private Bitmap _grayScaleFrameTMinus1;
        private byte[,] _imageTMinus1Mean;
        private byte[,] _imageTMinus1Variance;
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
                    ImageHelper.CalculateMeanAndVarianceM9ColorBitmap(
                        _grayScaleFrameTMinus1, 
                        out _imageTMinus1Mean,
                        out _imageTMinus1Variance);
                    _grayScaleBackground = (Bitmap)grayScaleImage.Clone();
                    return null;
                }

                byte[,] imageTMean;
                byte[,] imageTVariance;
                byte[,] resultData;
                using (
                    var temporalImageAnalysisResult = GetRadiometricSimmilarity(grayScaleImage, out resultData, out imageTMean,
                                                                                out imageTVariance))
                {
                    _interceptor.Intercept(RadiometricSimmilarity, temporalImageAnalysisResult);
                }

                _imageTMinus1Mean = imageTMean;
                _imageTMinus1Variance = imageTVariance;
                _grayScaleFrameTMinus1 = (Bitmap)grayScaleImage.Clone();
                return null;
            }
        }

        #endregion

        #region Private Methods

        // Radometric Simillarity returns doubles 0..1.
        // They are converted to grayscale bitmap for quantification and future analysis.
        private Bitmap GetRadiometricSimmilarity(Bitmap grayScaleFrameT, 
            out byte[,] dataHeigthWidth, 
            out byte[,] imageTMeanHeightWidth, 
            out byte[,] imageTVarianceHeightWidth)
        {
            ImageHelper.CalculateMeanAndVarianceM9ColorBitmap(
                grayScaleFrameT, 
                
                out imageTMeanHeightWidth, 
                out imageTVarianceHeightWidth);

            // Radiometric simillarity.
            GCHandle handle;
            var result = GrayScaleImageHelper.BeginImage(grayScaleFrameT.Width, grayScaleFrameT.Height, out dataHeigthWidth, out handle);
            long[,] firstItem;
            ImageHelper.CalculateMeanAndVarianceM9ColorBitmap2(grayScaleFrameT, _grayScaleFrameTMinus1, out firstItem);
            for (int width = 1; width < (grayScaleFrameT.Width-1); width++)
            {
                for (int height = 1; height < (grayScaleFrameT.Height-1); height++)
                {
                    dataHeigthWidth[height, width] =
                        (byte)((firstItem[height, width] - imageTMeanHeightWidth[height, width] * _imageTMinus1Mean[height, width]) /
                        Math.Sqrt(imageTVarianceHeightWidth[width, height]*_imageTMinus1Variance[height, width]));
                }
            }

            GrayScaleImageHelper.EndImage(handle);
            return result;
        }

        #endregion
    }
}