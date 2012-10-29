using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;
using HDE.IpCamClientServer.Server.Core.Profiling;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    /// <summary>
    /// "Background Subtraction and Shadow Detection in Grayscale Video Sequences"
    /// Julio Cezar Silveira Jacques Jr, Claudio Rosito Jung, Soraia Raupp Musse
    /// </summary>
    public class JulioClaudioSoraiaMovementDetector : MovementDetectorBase<JulioClaudioSoraiaMovementDetector.JulioClaudioSoraiaMovementDetectorBackgroundModel>
    {
        #region Constants

        private const string MinIntensityBackgroundDebugView = "Background model: minimum intensity";
        private const string MaxIntensityBackgroundDebugView = "Background model: maximum intensity";
        private const string MaxPerFrameDifferenceDebugView = "Background model: maximum per frame difference";

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

            public void Train(Bitmap grayscaleFrame)
            {
                if (IsOperational())
                {
                    throw new InvalidDataException("Model is operational.");
                }
                _amountOfTrainingFramesLeft--;
                
                var bounds = new Rectangle(0, 0, grayscaleFrame.Width, grayscaleFrame.Height);
                BitmapData bitmapData = grayscaleFrame.LockBits(bounds, ImageLockMode.ReadOnly, grayscaleFrame.PixelFormat);
                var grayScaleHW = new byte[grayscaleFrame.Height * bitmapData.Stride];
                Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayscaleFrame.Height * bitmapData.Stride);
                _stride = bitmapData.Stride;
                grayscaleFrame.UnlockBits(bitmapData);

                _trainingDataNHW[_amountOfTrainingFramesLeft] = grayScaleHW;

                _width = grayscaleFrame.Width;
                _height = grayscaleFrame.Height;

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

                        long medium = 0;
                        long disperce = 0;
                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            medium += _trainingDataNHW[frameNo][position];
                        }
                        medium = medium / _amountOfTrainingFrames;

                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            disperce += Math.Abs(_trainingDataNHW[frameNo][position] - medium);
                        }
                        disperce = disperce / _amountOfTrainingFrames;

                        var minIntensity = byte.MaxValue;
                        var maxIntensity = byte.MinValue;
                        var maxPerFrameDifference = byte.MinValue;

                        int previousTrainingValue = -1;
                        for (int frameNo = 0; frameNo < _amountOfTrainingFrames; frameNo++)
                        {
                            var val = _trainingDataNHW[frameNo][position];
                            if (Math.Abs(val - medium) <= 2 * disperce)
                            {
                                if (val < minIntensity)
                                {
                                    minIntensity = val;
                                }
                                if (val > maxIntensity)
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

                        _minIntensity[position] = minIntensity;
                        _maxIntensity[position] = maxIntensity;
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
                           MinIntensityBackgroundDebugView, 
                           MaxIntensityBackgroundDebugView, 
                           MaxPerFrameDifferenceDebugView
                       };
        }

        public override void Configure(string configurationString)
        {
            base.Configure(configurationString);

            if (configurationString.Contains(";DEBUG;"))
            {
                //_backgroundModel.Initialize(100);
                //_backgroundModel.Initialize(900);//TEST
                _backgroundModel.Initialize(4);
            }
            else
            {
                _backgroundModel.Initialize(80 * 25);
            }
        }

        protected override string ProcessInternal(Bitmap bitmap)
        {
            //using (new ProfilerScene())
            using (var grayScaleImage = GrayScaleImageHelper.ToGrayScale(bitmap))
            {
                if (!_backgroundModel.IsOperational())
                {
                    _backgroundModel.Train(grayScaleImage);
                    return null;
                }

                while (true)
                {
                    _interceptor.Intercept(MinIntensityBackgroundDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._minIntensity));
                    _interceptor.Intercept(MaxIntensityBackgroundDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._maxIntensity));
                    _interceptor.Intercept(MaxPerFrameDifferenceDebugView, GrayScaleImageHelper.FromData2(_backgroundModel._width, _backgroundModel._height, _backgroundModel._stride, _backgroundModel._maxPerFrameDifference));
                }
            }
            return null;
        }
    }
}