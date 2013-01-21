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
        private const string XXX = "XXX";
        private const string DetectedBlobDebugView = "Movement Detections";

        private const string DifferenceDebugView = "Difference";

        private int _minimumDetectionWidthPixels;
        private int _minimumDetectionHeightPixels;
        private int _maximumDetectionWidthPixels;
        private int _maximumDetectionHeightPixels;

        private int _regionFrameSizeDivided2;
        private int _regionProveShadowDivided2;
        private int _borderDivided2;
        private double _Llow;
        private double _Lstd;
        private double _Lncc;

        #endregion

        #region Nested Types

        public class JulioClaudioSoraiaMovementDetectorBackgroundModel
        {
            #region Constants

            protected byte _dK;
            
            #endregion

            #region Fields

            private bool _siarheiKuchuk;

            #region Background Substraction Related

            internal byte[] _minIntensity;
            internal byte[] _maxIntensity;
            internal byte[] _maxPerFrameDifference;

            #endregion

            #region Shadow Elimination Related

            internal byte[] _averageBackground;
            internal double[] _energyBackground;

            #endregion


            private byte[][] _trainingDataNHW;
            private int _amountOfTrainingFrames;
            private int _amountOfTrainingFramesLeft = -1;

            private int _regionFrameSizeDivided2;

            internal int _width;
            internal int _height;
            internal int _stride;

            #endregion

            #region Public Methods

            public void Initialize(
                int amountOfTrainingFrames, 
                int regionFrameSizeDivided2,
                byte dk, 
                bool siarheiKuchuk)
            {
                if (amountOfTrainingFrames < 2)
                {
                    throw new ArgumentOutOfRangeException("amountOfTrainingFrames");
                }

                if (regionFrameSizeDivided2 < 2)
                {
                    throw new ArgumentOutOfRangeException("regionFrameSizeDivided2");
                }

                _siarheiKuchuk = siarheiKuchuk;
                _amountOfTrainingFrames = amountOfTrainingFrames;
                _amountOfTrainingFramesLeft = amountOfTrainingFrames;
                _regionFrameSizeDivided2 = regionFrameSizeDivided2;
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

                if (_averageBackground == null)
                {
                    _averageBackground = new byte[grayScaleHW.Length];
                }

                if (_energyBackground == null)
                {
                    _energyBackground = new double[grayScaleHW.Length];
                }

                if (_amountOfTrainingFramesLeft == 0)
                {
                    CalculateInitialBackgroundModelState();
                }
            }

            #endregion

            #region Private Methods

            private void CalculateEnergyBackground()
            {
                ArrayHelper.SetToAll(_energyBackground, 0);
                for (int widthI = _regionFrameSizeDivided2; widthI < (_width - _regionFrameSizeDivided2 - 1); widthI++)
                {
                    for (int heightI = _regionFrameSizeDivided2; heightI < (_height - _regionFrameSizeDivided2 - 1); heightI++)
                    {
                        long result = 0;

                        for (
                             int regionWidthI = (widthI - _regionFrameSizeDivided2); 
                             regionWidthI < (widthI + _regionFrameSizeDivided2 + 1);
                             regionWidthI++)
                        {
                            for (
                             int regionHeightI = (heightI - _regionFrameSizeDivided2); 
                             regionHeightI < (heightI + _regionFrameSizeDivided2 + 1);
                             regionHeightI++)
                            {
                                result += _averageBackground[GrayScaleImageHelper.ToDataPosition(regionWidthI, regionHeightI, _stride)];
                            }
                        }

                        _energyBackground[GrayScaleImageHelper.ToDataPosition(widthI, heightI, _stride)] = Math.Sqrt(result);
                    }
                }
            }

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

                        _averageBackground[position] = (byte)medium;

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

                CalculateEnergyBackground();

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

                           //MinIntensityBackgroundDebugView, 
                           //MaxIntensityBackgroundDebugView, 
                           MaxPerFrameDifferenceDebugView,
                           XXX,
                           DifferenceDebugView,
                           DetectedBlobDebugView
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
            InitializeBackgroundModel(
                int.Parse(settings["TrainingFrames"], CultureInfo.InvariantCulture),
                int.Parse(settings["N"], CultureInfo.InvariantCulture));
            _minimumDetectionWidthPixels = int.Parse(settings["MinimumDetectionWidthPixels"], CultureInfo.InvariantCulture);
            _minimumDetectionHeightPixels = int.Parse(settings["MinimumDetectionHeightPixels"], CultureInfo.InvariantCulture);
            _maximumDetectionWidthPixels = int.Parse(settings["MaximumDetectionWidthPixels"], CultureInfo.InvariantCulture);
            _maximumDetectionHeightPixels = int.Parse(settings["MaximumDetectionHeightPixels"], CultureInfo.InvariantCulture);
            _regionFrameSizeDivided2 = int.Parse(settings["N"], CultureInfo.InvariantCulture);
            _regionProveShadowDivided2 = int.Parse(settings["M"], CultureInfo.InvariantCulture)*2;
            _borderDivided2 = Math.Max(_regionFrameSizeDivided2, _regionProveShadowDivided2);
            _Llow = double.Parse(settings["Llow"], CultureInfo.InvariantCulture);
            _Lstd = double.Parse(settings["Lstd"], CultureInfo.InvariantCulture);
            _Lncc = double.Parse(settings["Lncc"], CultureInfo.InvariantCulture);
        }

        protected virtual void InitializeBackgroundModel(int trainingFrames, int regionFrameSizeDivided2)
        {
            _backgroundModel.Initialize(trainingFrames, regionFrameSizeDivided2, 2, false);
        }

        protected virtual void PreprocessFrame(byte[] dataHW, int stride, int width, int height)
        {

        }

        protected virtual short[,] CreateClosingOpeningFileter()
        {
            return new short[,]
                {
                    {-1, -1, +1, -1, -1},
                    {-1, +1, +1, +1, -1},
                    {+1, +1, +1, +1, +1},
                    {-1, +1, +1, +1, -1},
                    {-1, -1, +1, -1, -1},
                };
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

                //excluded, because does not work.
                //ExcludeShadows(grayScaleHW, foreground, grayScaleImage.Width, grayScaleImage.Height, stride);

                var temp = GrayScaleImageHelper.FromData(
                    _backgroundModel._width,
                    _backgroundModel._height,
                    _backgroundModel._stride,
                    foreground);

                int countDetected = 0;
                // create filter
                var diamondMask = CreateClosingOpeningFileter();
                Closing filter = new Closing(diamondMask);
                Opening filter2 = new Opening(diamondMask);
                // apply the filter
                filter.ApplyInPlace(temp);
                filter2.ApplyInPlace(temp);
                _interceptor.Intercept(
                    XXX, 
                    ImageHelper.ToBytes(temp));

                // blobs!
                var bc = new BlobCounter {BackgroundThreshold = Color.FromArgb(254, 254, 254)};
                bc.ProcessImage(temp);

                Rectangle[] rects = bc.GetObjectsRectangles();
                var rectanglesToDraw = new List<Rectangle>();
                foreach (Rectangle rect in rects)
                {
                    if (rect.Width < _maximumDetectionWidthPixels &&
                        rect.Width > _minimumDetectionWidthPixels &&
                        rect.Height < _maximumDetectionHeightPixels &&
                        rect.Height > _minimumDetectionHeightPixels)
                    {
                        countDetected++;
                        rectanglesToDraw.Add(rect);
                    }
                }
                temp.Dispose();
                
                _interceptor.Intercept(InputFrameDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, grayScaleHW));
                _interceptor.Intercept(DifferenceDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, foreground));

                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (var brush = new SolidBrush(Color.Red))
                using (var pen = new Pen(brush, 3))
                {
                    if (rectanglesToDraw.Any())
                    {
                        graphics.DrawRectangles(pen, rectanglesToDraw.ToArray());
                    }
                    _interceptor.Intercept(DetectedBlobDebugView, ImageHelper.ToBytes(bitmap));
                }

                if (countDetected > 0)
                {
                    return "Alarm: " + countDetected;
                }
            }
            return null;
        }

        private void ExcludeShadows(byte[] frame, byte[] foreground, int width, int height, int stride)
        {
            for (int widthI = _borderDivided2; widthI < (width - _borderDivided2 - 1); widthI++)
            {
                for (int heightI = _borderDivided2; heightI < (height - _borderDivided2 - 1); heightI++)
                {
                    var pos = GrayScaleImageHelper.ToDataPosition(widthI, heightI, stride);
                    if (foreground[pos] == _foregroundPixel &&
                        CheckProveShadow3(frame[pos], _backgroundModel._averageBackground[pos]) &&
                        CheckProveShadow2(frame, widthI, heightI, stride) &&
                        CheckProveShadowOriginal(frame, widthI, heightI, stride))
                    {
                        foreground[pos] = _backgroundPixel;
                    }
                }
            }
        }

        private bool CheckProveShadowOriginal(byte[] frame, int widthI, int heightI, int stride)
        {
            double er = 0;
            double eb = _backgroundModel._energyBackground[GrayScaleImageHelper.ToDataPosition(widthI, heightI, stride)];
            double et = 0;
            for (int regionWidthI = widthI - _regionFrameSizeDivided2;
                regionWidthI < (widthI + _regionFrameSizeDivided2 + 1);
                regionWidthI++)
            {

                for (
                    int regionHeightI = (heightI - _regionFrameSizeDivided2);
                    regionHeightI < (heightI + _regionFrameSizeDivided2 + 1);
                    regionHeightI++)
                {
                    var posCheckRegion = GrayScaleImageHelper.ToDataPosition(regionWidthI, regionHeightI, stride);
                    er += _backgroundModel._averageBackground[posCheckRegion] * frame[posCheckRegion];
                    et += frame[posCheckRegion]*frame[posCheckRegion];
                }
            }
            et = Math.Sqrt(et);

            var ncc = er/(eb*et);
            return ncc > _Lncc && et < eb;
        }

        private bool CheckProveShadow2(byte[] frame, int widthI, int heightI, int stride)
        {
            var standardDeviationOfRegion = new List<double>();

            for (int regionWidthI = widthI - _regionProveShadowDivided2;
                regionWidthI < (widthI + _regionProveShadowDivided2 + 1);
                regionWidthI++)
            {
                for (
                    int regionHeightI = (heightI - _regionProveShadowDivided2);
                    regionHeightI < (heightI + _regionProveShadowDivided2 + 1);
                    regionHeightI++)
                {
                    var posCheckRegion = GrayScaleImageHelper.ToDataPosition(regionWidthI, regionHeightI, stride);
                    if (_backgroundModel._averageBackground[posCheckRegion] != 0)
                    {
                        standardDeviationOfRegion.Add((double)frame[posCheckRegion] / _backgroundModel._averageBackground[posCheckRegion]);
                    }
                }
            }

            var deviation = ArrayHelper.GetStandardDeviation(standardDeviationOfRegion.ToArray());
            return deviation < _Lstd;
        }

        private bool CheckProveShadow3(byte foreground, byte background)
        {
            if (background == 0)
            {
                return false;
            }
            var divided = ((double)foreground)/background;
            return (divided < 1 && divided >= _Llow);
        }

        private byte _backgroundPixel = byte.MinValue;
        private byte _foregroundPixel = byte.MaxValue;

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
                        foregroundHW[position] = _backgroundPixel; // white
                    }

                    else
                    {
                        foregroundHW[position] = _foregroundPixel;
                    }
                }
            }
            return foregroundHW;
        }
    }
}