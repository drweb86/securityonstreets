using System;
using System.Drawing;
using System.IO;
using AForge.Imaging.Filters;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public class MovementHandler: IHandler
    {
        #region Fields

        private readonly ThresholdedDifference _differenceFilter;
        private readonly int _reportDifference;
        private readonly int _threshold;

        #endregion

        #region Constructors

        public MovementHandler()
        {
//TODO: configurate it.
            const int difference = 60;
            const int reportDifference = 5000;
            _differenceFilter = new ThresholdedDifference(difference);
            _reportDifference = reportDifference;
            _threshold = difference;
        }

        #endregion

        #region IHandler Implementation

        public string Process(Bitmap bitmap)
        {
            if (_differenceFilter.OverlayImage == null)
            {
                _differenceFilter.OverlayImage = (Bitmap)bitmap.Clone();
                return null;
            }
            using (_differenceFilter.Apply(bitmap))
            {
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
