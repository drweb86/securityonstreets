using System;
using AForge.Video;
using HDE.Platform.Logging;
using Hde.StreetWatch.Controller;
using Hde.StreetWatch.Model;

namespace Hde.StreetWatch
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var controller = new SWController(new SWModel());
            try
            {
                controller.Initialise();
                AForge.Video.
                JPEGStream stream = new JPEGStream(@"");
                stream.Start();
                controller.PlayVideo(stream);
                stream.WaitForStop();
                
            }
            catch (Exception unhandledException)
            {
                controller.Log.WriteLine(LoggingEvent.Error, unhandledException.ToString());
            }
            finally
            {
                controller.TearDown();
            }
        }

        static void stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            eventArgs.Frame
        }
    }
}
