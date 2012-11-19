using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AForge.Imaging;
using AForge.Imaging.Filters;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    /// <summary>
    /// "Background Subtraction and Shadow Detection in Grayscale Video Sequences"
    /// Julio Cezar Silveira Jacques Jr, Claudio Rosito Jung, Soraia Raupp Musse
    /// </summary>
    public class JulioClaudioSoraiaMovementDetector : MovementDetectorBase<JulioClaudioSoraiaMovementDetector.JulioClaudioSoraiaMovementDetectorBackgroundModel>
    {
        #region Fields

        private int _k = -1;

        #endregion

        #region Constants

        private const string InputFrameDebugView = "Input Frame";

        private const string MinIntensityBackgroundDebugView = "Background model: minimum intensity";
        private const string MaxIntensityBackgroundDebugView = "Background model: maximum intensity";
        private const string MaxPerFrameDifferenceDebugView = "Background model: maximum per frame difference";

        private const string DifferenceDebugView = "Difference";

        #endregion

        #region Nested Types

        public class JulioClaudioSoraiaMovementDetectorBackgroundModel
        {
            #region Fields

            internal byte[] _minIntensity;
            internal byte[] _maxIntensity;
            internal byte[] _maxPerFrameDifference;

            private byte[][] _trainingDataNHW;
            private int _amountOfTrainingFrames;
            private int _amountOfTrainingFramesLeft = -1;

            internal int _width;
            internal int _height;
            internal int _stride;

            #endregion

            #region Public Methods

            public void Initialize(int amountOfTrainingFrames)
            {
                if (amountOfTrainingFrames < 2)
                {
                    throw new ArgumentOutOfRangeException("amountOfTrainingFrames");
                }

                _amountOfTrainingFrames = amountOfTrainingFrames;
                _amountOfTrainingFramesLeft = amountOfTrainingFrames;
                _trainingDataNHW = new byte[_amountOfTrainingFrames][];
                _minIntensity = null;
                _maxIntensity = null;
                _maxPerFrameDifference = null;
            }

            public bool IsOperational()
            {
                return _amountOfTrainingFramesLeft == 0;
            }

            public void Train(byte[] grayScaleHW, int width, int height, int stride)
            {
                if (IsOperational())
                {
                    throw new InvalidDataException("Model is operational.");
                }
                _amountOfTrainingFramesLeft--;

                _stride = stride;
                _width = width;
                _height = height;

                _trainingDataNHW[_amountOfTrainingFramesLeft] = grayScaleHW;

                if (_minIntensity == null)
                {
                    _minIntensity = new byte[grayScaleHW.Length];
                }
                if (_maxIntensity == null)
                {
                    _maxIntensity = new byte[grayScaleHW.Length];
                }
                if (_maxPerFrameDifference == null)
                {
                    _maxPerFrameDifference = new byte[grayScaleHW.Length];
                }

                if (_amountOfTrainingFramesLeft == 0)
                {
                    CalculateInitialBackgroundModelState();
                }
            }

            #endregion

            #region Private Methods

            private void CalculateInitialBackgroundModelState()
            {
                for (int widthI = 0; widthI < _width; widthI++)
                {
                    for (int heightI = 0; heightI < _height; heightI++)
                    {
                        var position = GrayScaleImageHelper.ToDataPosition(widthI, heightI, _stride);

                        double medium = 0;
                        double disperce = 0;
                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            medium += _trainingDataNHW[frameNo][position];
                        }
                        medium = (1.0*medium) / _amountOfTrainingFrames;

                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            disperce += Math.Abs(_trainingDataNHW[frameNo][position] - medium);
                        }
                        disperce = (1.0*disperce) / _amountOfTrainingFrames;

                        var minIntensity = -1;
                        var maxIntensity = -1;
                        var maxPerFrameDifference = byte.MinValue;

                        int previousTrainingValue = -1;
                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            var val = _trainingDataNHW[frameNo][position];
                            if (Math.Abs(val - medium) <= 2 * disperce)
                            {
                                if (minIntensity == -1 ||
                                    val < minIntensity)
                                {
                                    minIntensity = val;
                                }
                                if (maxIntensity == -1 ||
                                    val > maxIntensity)
                                {
                                    maxIntensity = val;
                                }

                                if (previousTrainingValue != -1)
                                {
                                    var difference = (byte) Math.Abs(val - previousTrainingValue);
                                    if (difference > maxPerFrameDifference)
                                    {
                                        maxPerFrameDifference = difference;
                                    }
                                }
                                previousTrainingValue = val;
                            }
                        }

                        _minIntensity[position] = (byte)minIntensity;
                        _maxIntensity[position] = (byte)maxIntensity;
                        _maxPerFrameDifference[position] = maxPerFrameDifference;
                    }
                }
                _trainingDataNHW = null;
            }

            #endregion
        }

        #endregion

        #region Fields

        #endregion

        public JulioClaudioSoraiaMovementDetector(IInterceptor interceptor) : base(interceptor)
        {
        }

        public override string[] GetDebugWindows()
        {
            return new []
                       {
                           InputFrameDebugView,

                           //MinIntensityBackgroundDebugView, 
                           //MaxIntensityBackgroundDebugView, 
                           //MaxPerFrameDifferenceDebugView,

                           DifferenceDebugView
                       };
        }

        public override void Configure(string configurationString)
        {
            base.Configure(configurationString);

            var settings = configurationString
                .Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(item=>item[0], item=> item.Length > 1 ? item[1] : null);

            _backgroundModel.Initialize(int.Parse(settings["TrainingFrames"], CultureInfo.InvariantCulture));

            _k = int.Parse(settings["K"], CultureInfo.InvariantCulture);
        }

        protected virtual void PreprocessFrame(Bitmap bitmap)
        {

        }

        protected override string ProcessInternal(Bitmap bitmap)
        {
            using (var grayScaleImage = GrayScaleImageHelper.ToGrayScale(bitmap))
            {
                PreprocessFrame(grayScaleImage);
                
                var bounds = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
                BitmapData bitmapData = grayScaleImage.LockBits(bounds, ImageLockMode.ReadOnly, grayScaleImage.PixelFormat);
                var grayScaleHW = new byte[grayScaleImage.Height * bitmapData.Stride];
                Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayScaleImage.Height * bitmapData.Stride);
                var stride = bitmapData.Stride;
                grayScaleImage.UnlockBits(bitmapData);

                if (!_backgroundModel.IsOperational())
                {
                    _backgroundModel.Train(grayScaleHW, grayScaleImage.Width, grayScaleImage.Height, stride);
                    
                    if (_backgroundModel.IsOperational())
                    {
                        _interceptor.Intercept(MinIntensityBackgroundDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._minIntensity));
                        _interceptor.Intercept(MaxIntensityBackgroundDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._maxIntensity));
                        _interceptor.Intercept(MaxPerFrameDifferenceDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._maxPerFrameDifference));
                    }

                    return null;
                }

                int amountOfWhitePixels;
                var foreground = GetForefround(grayScaleHW, grayScaleImage.Width, grayScaleImage.Height, stride, out amountOfWhitePixels);


                var temp = GrayScaleImageHelper.FromData(
                _backgroundModel._width,
                _backgroundModel._height,
                _backgroundModel._stride,
                foreground);
                var bc = new BlobCounter(temp) {ObjectsOrder = ObjectsOrder.Size};
                var objects = bc.GetObjects(temp, false);
                var countDetected = objects.Count(item => item.Rectangle.Height > 4 && item.Rectangle.Width > 4);
                
                _interceptor.Intercept(InputFrameDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, grayScaleHW));
                _interceptor.Intercept(DifferenceDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, foreground));

                if (countDetected > 0)
                {
                    return "Alarm: " + countDetected;
                }

            }
            return null;
        }

        private byte[] GetForefround(
            byte[] grayScaleHW,
            int width,
            int height,
            int stride,
            out int amountOfBlackPixels)
        {
            amountOfBlackPixels = 0;
            var foregroundHW = new byte[grayScaleHW.Length];
            for (int widthI = 0; widthI < width; widthI++)
            {
                for (int heightI = 0; heightI < height; heightI++)
                {
                    var position = GrayScaleImageHelper.ToDataPosition(widthI, heightI, stride);
                    var threshold = _k * _backgroundModel._maxPerFrameDifference[position];
                    var pixel = grayScaleHW[position];
                    foregroundHW[position] = byte.MaxValue;

                    if (_backgroundModel._minIntensity[position] - threshold <= pixel &&
                            pixel <= _backgroundModel._maxIntensity[position] + threshold)
                    {
                        foregroundHW[position] = byte.MaxValue; // white
                    }

                    else
                    {
                        foregroundHW[position] = byte.MinValue;
                        amountOfBlackPixels++;
                    }
                }
            }
            return foregroundHW;
        }
    }
}