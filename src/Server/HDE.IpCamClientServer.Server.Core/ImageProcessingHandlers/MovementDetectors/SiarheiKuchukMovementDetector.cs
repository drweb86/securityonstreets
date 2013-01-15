using System;
using System.Globalization;
using System.Linq;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public class SiarheiKuchukMovementDetector : JulioClaudioSoraiaMovementDetector
    {
        #region Fields

        private int _closingOpeningMatrixSize;

        #endregion

        public SiarheiKuchukMovementDetector(IInterceptor interceptor) : base(interceptor)
        {
        }

        public override void Configure(string configurationString)
        {
            base.Configure(configurationString);
            var settings = configurationString
                .Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(item=>item[0], item=> item.Length > 1 ? item[1] : null);

            _closingOpeningMatrixSize = int.Parse(settings["ClosingOpeningMatrixSize"], CultureInfo.InvariantCulture);

            if (_closingOpeningMatrixSize < 2)
            {
                throw new ArgumentOutOfRangeException("ClosingOpeningMatrixSize");
            }
            _closingOpeningMatrixSize = (_closingOpeningMatrixSize / 2) * 2 + 1;
        }

        protected override void InitializeBackgroundModel(int trainingFrames, int regionFrameSizeDivided2)
        {
            _backgroundModel.Initialize(trainingFrames, regionFrameSizeDivided2, (byte)_k, true);
        }

        protected override void PreprocessFrame(byte[] dataHW, int stride, int width, int height)
        {
            if (!_backgroundModel.IsOperational())
            {
                GrayScaleImageHelper.ApplySaltAndPapperNoise(2, dataHW, stride, width, height);
            }
        }

        protected override short[,] CreateClosingOpeningFileter()
        {
            var diamondSizeDiv2Minus1 = (_closingOpeningMatrixSize - 1) / 2;

            var diamondSizeMinus1 = diamondSizeDiv2Minus1 * 2;

            var result = new short[diamondSizeMinus1 + 1, diamondSizeMinus1 + 1];
            for (int i = 0; i <= diamondSizeMinus1; i++)
            {
                for (int j = 0; j <= diamondSizeMinus1; j++)
                {
                    if (i == diamondSizeDiv2Minus1 || j == diamondSizeDiv2Minus1)
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        var relativePosI = i > diamondSizeDiv2Minus1 ? diamondSizeMinus1 - i : i;
                        var relativePosJ = j > diamondSizeDiv2Minus1 ? diamondSizeMinus1 - j : j;

                        result[i, j] = (relativePosJ < diamondSizeDiv2Minus1 - relativePosI) ? (short)-1 : (short)1;
                    }
                }
            }
            return result;
        }
    }
}