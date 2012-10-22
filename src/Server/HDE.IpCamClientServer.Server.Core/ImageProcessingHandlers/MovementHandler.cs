using System;
using System.Drawing;
using AForge.Imaging.Filters;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public class MovementHandler: IHandler
    {
        #region Constants

        public const string DifferenceImage = "DIFF";

        #endregion

        #region Fields

        private readonly ThresholdedDifference _differenceFilter;
        private readonly int _reportDifference;
        private readonly int _threshold;
        private readonly IInterceptor _interceptor;

        #endregion

        #region Constructors

        public MovementHandler(IInterceptor interceptor = null)
        {
//TODO: configurate it.
            const int difference = 60;
            const int reportDifference = 5000;
            _differenceFilter = new ThresholdedDifference(difference);
            _reportDifference = reportDifference;
            _threshold = difference;
            _interceptor = interceptor ?? new NullInterceptor();
        }

        #endregion

        #region IHandler Implementation

        public string[] GetDebugWindows()
        {
            return new[] {DifferenceImage};
        }

        public void Configure(string configurationString)
        {
            //TODO:
        }

        public string Process(Bitmap bitmap)
        {
            if (_differenceFilter.OverlayImage == null)
            {
                _differenceFilter.OverlayImage = (Bitmap)bitmap.Clone();
                return null;
            }
            using (var difference = _differenceFilter.Apply(bitmap))
            {
                _interceptor.Intercept(DifferenceImage, ImageHelper.ToBytes(difference));

                if (_differenceFilter.WhitePixelsCount > _reportDifference)
                {
                    return DateTime.Now.ToString("g") + " Frame difference is " + _differenceFilter.WhitePixelsCount + " pixels, filter threthhold " + _threshold;
                }
            }

            return null;
        }

        #endregion
    }
}
