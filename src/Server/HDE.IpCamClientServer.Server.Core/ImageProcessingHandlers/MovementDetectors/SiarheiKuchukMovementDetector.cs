using System;
using System.Globalization;
using System.Linq;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public class SiarheiKuchukMovementDetector : JulioClaudioSoraiaMovementDetector
    {
        #region Fields

        private int _closingOpeningMatrixSize;
        private int _comparisonCCoefficient;

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

            _closingOpeningMatrixSize = Convert.ToInt32(
                Math.Max(
                    3, 
                    0.02*int.Parse(settings["MaximumDetectionHeightPixels"], CultureInfo.InvariantCulture)));

            if (_closingOpeningMatrixSize < 3)
            {
                throw new ArgumentOutOfRangeException("ClosingOpeningMatrixSize");
            }
            _closingOpeningMatrixSize = (_closingOpeningMatrixSize / 2) * 2 + 1;
            _comparisonCCoefficient = int.Parse(settings["comparisonCCoefficient"]);
        }

        protected override void InitializeBackgroundModel(int trainingFrames, int regionFrameSizeDivided2)
        {
            _backgroundModel.Initialize(trainingFrames, regionFrameSizeDivided2, (byte)_k, true);
        }

        protected override double GetThreshold(int position)
        {
            return _comparisonCCoefficient + base.GetThreshold(position);
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