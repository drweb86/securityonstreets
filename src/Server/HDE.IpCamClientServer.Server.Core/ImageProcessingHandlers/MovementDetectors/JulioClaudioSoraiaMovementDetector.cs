using System;
using System.Collections.Generic;
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

        protected int _k = -1;

        #endregion

        #region Constants

        private const string InputFrameDebugView = "Input Frame";

        private const string MinIntensityBackgroundDebugView = "Background model: minimum intensity";
        private const string MaxIntensityBackgroundDebugView = "Background model: maximum intensity";
        private const string MaxPerFrameDifferenceDebugView = "Background model: maximum per frame difference";

        private const string DifferenceDebugView = "Difference";

        private int _minimumDetectionWidthPixels;
        private int _minimumDetectionHeightPixels;
        private int _maximumDetectionWidthPixels;
        private int _maximumDetectionHeightPixels;

        #endregion

        #region Nested Types

        public class JulioClaudioSoraiaMovementDetectorBackgroundModel
        {
            #region Constants

            protected byte _dK;
            
            #endregion

            #region Fields

            private bool _siarheiKuchuk;
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

            public void Initialize(int amountOfTrainingFrames, byte dk, bool siarheiKuchuk)
            {
                if (amountOfTrainingFrames < 2)
                {
                    throw new ArgumentOutOfRangeException("amountOfTrainingFrames");
                }
                _siarheiKuchuk = siarheiKuchuk;
                _amountOfTrainingFrames = amountOfTrainingFrames;
                _amountOfTrainingFramesLeft = amountOfTrainingFrames;
                _trainingDataNHW = new byte[_amountOfTrainingFrames][];
                _minIntensity = null;
                _maxIntensity = null;
                _maxPerFrameDifference = null;
                _dK = dk;
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
                        if (_siarheiKuchuk)
                        {
                            for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                            {
                                medium += _trainingDataNHW[frameNo][position];
                            }
                            medium = (1.0*medium)/_amountOfTrainingFrames;
                        }
                        else
                        {
                            var list = new List<byte>();
                            for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                            {
                                list.Add( _trainingDataNHW[frameNo][position]);
                            }
                            medium = ArrayHelper.GetMedian(list.ToArray());
                        }

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
                            if (Math.Abs(val - medium) <= _dK * disperce) 
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

                        if (minIntensity == -1)
                        {
                            throw new InvalidDataException("min intensity!");
                        }

                        if (maxIntensity == -1)
                        {
                            throw new InvalidDataException("max intensity!");
                        }

                        _minIntensity[position] = (byte)minIntensity;
                        _maxIntensity[position] = (byte)maxIntensity;
                        _maxPerFrameDifference[position] = maxPerFrameDifference;
                    }
                }

                DumpTrainingData();

                _trainingDataNHW = null;
            }

            private void DumpTrainingData()
            {
                //for (int widthI = 0; widthI < _width; widthI++)
                //{
                //    for (int heightI = 0; heightI < _height; heightI++)
                //    {
                //        var position = GrayScaleImageHelper.ToDataPosition(widthI, heightI, _stride);
                //        GrayScaleImageHelper.FromData2()
                //        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                //        {
                //            var val = _trainingDataNHW[frameNo][position];
                //        }
                //    }
                //}
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

                           MinIntensityBackgroundDebugView, 
                           MaxIntensityBackgroundDebugView, 
                           MaxPerFrameDifferenceDebugView,

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

            _k = int.Parse(settings["K"], CultureInfo.InvariantCulture);
            InitializeBackgroundModel(int.Parse(settings["TrainingFrames"], CultureInfo.InvariantCulture));
            _minimumDetectionWidthPixels = int.Parse(settings["MinimumDetectionWidthPixels"], CultureInfo.InvariantCulture);
            _minimumDetectionHeightPixels = int.Parse(settings["MinimumDetectionHeightPixels"], CultureInfo.InvariantCulture);
            _maximumDetectionWidthPixels = int.Parse(settings["MaximumDetectionWidthPixels"], CultureInfo.InvariantCulture);
            _maximumDetectionHeightPixels = int.Parse(settings["MaximumDetectionHeightPixels"], CultureInfo.InvariantCulture);
        }

        protected virtual void InitializeBackgroundModel(int trainingFrames)
        {
            _backgroundModel.Initialize(trainingFrames, 2, false);
        }

        protected virtual void PreprocessFrame(byte[] dataHW, int stride, int width, int height)
        {

        }

        protected override string ProcessInternal(Bitmap bitmap)
        {
            using (var grayScaleImage = GrayScaleImageHelper.ToGrayScale(bitmap))
            {
                var bounds = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
                BitmapData bitmapData = grayScaleImage.LockBits(bounds, ImageLockMode.ReadOnly, grayScaleImage.PixelFormat);
                var grayScaleHW = new byte[grayScaleImage.Height * bitmapData.Stride];
                Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayScaleImage.Height * bitmapData.Stride);
                var stride = bitmapData.Stride;
                grayScaleImage.UnlockBits(bitmapData);

                PreprocessFrame(grayScaleHW, stride, grayScaleImage.Width, grayScaleImage.Height);

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

                var foreground = GetForefround(grayScaleHW, grayScaleImage.Width, grayScaleImage.Height, stride);

                var temp = GrayScaleImageHelper.FromData(
                _backgroundModel._width,
                _backgroundModel._height,
                _backgroundModel._stride,
                foreground);

                int countDetected = 0;
//TODO: increase
//TODO: decrease
//TODO: !
                //// skelet!
                //var skelet = new SimpleSkeletonization {Background = byte.MinValue, Foreground = byte.MaxValue};
                //skelet.ApplyInPlace(temp);

                // blobs!
                var bc = new BlobCounter {BackgroundThreshold = Color.FromArgb(254, 254, 254)};
                bc.ProcessImage(temp);

                Rectangle[] rects = bc.GetObjectsRectangles();
                foreach (Rectangle rect in rects)
                {
                    if (rect.Width < _maximumDetectionWidthPixels &&
                        rect.Width > _minimumDetectionWidthPixels &&
                        rect.Height < _maximumDetectionHeightPixels &&
                        rect.Height > _minimumDetectionHeightPixels)
                    {
                        countDetected++;
                    }
                }
                
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
            int stride)
        {
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
                        foregroundHW[position] = byte.MinValue; // white
                    }

                    else
                    {
                        foregroundHW[position] = byte.MaxValue;
                    }
                }
            }
            return foregroundHW;
        }
    }
}