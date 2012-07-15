using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Video;
using HDE.Platform.Logging;
using Hde.StreetWatch.Controller;
using Hde.StreetWatch.View;

namespace Hde.StreetWatch.Commands
{
    class PlayVideoCmd
    {
        public void PlayVideo(SWController swController, JPEGStream jpegStream)
        {
            IViewVideo form = null;
            try
            {
                form = (IViewVideo)swController.Factory.Get(typeof(IViewVideo));
                form.Process();

                jpegStream.NewFrame += (s, e) => form.DisplayFrame(e.Frame);
                jpegStream.PlayingFinished += (s, e) => form.Finish();
            }
            catch (Exception e)
            {
                swController.Log.WriteLine(LoggingEvent.Error, e.ToString());
                if (form != null)
                {
                    form.Finish();
                }

                throw;
            }

            
        }
    }
}
