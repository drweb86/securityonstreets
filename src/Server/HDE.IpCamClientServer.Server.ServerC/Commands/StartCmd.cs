using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using HDE.IpCamClientServer.Common.Messaging;
using HDE.IpCamClientServer.Server.ServerC.Controller;
using MessageRouter.Common;

namespace HDE.IpCamClientServer.Server.ServerC.Commands
{
    class StartCmd
    {
        #region Public Methods

        public void Start(ServerController controller)
        {
            controller.Model.WorkerThread = new Thread(DoWork);
            controller.Model.WorkerThread.Start(controller);
        }

        #endregion

        #region Private Methods

        private void DoWork(object context)
        {
            var controller = (ServerController) context;

            using (var results = new MessageRouterResults(controller.Log, controller.Model.Settings.TargetQueueConfig))
            {
                controller.Log.Info("Connecting to {0}", controller.Model.Settings.CameraConnection.Title);

                var mjpegConnection = new MJPEGStream
                                          {
                                              Source = controller.Model.Settings.CameraConnection.Uri,
                                              Login = controller.Model.Settings.CameraConnection.User,
                                              Password = controller.Model.Settings.CameraConnection.Password,
                                          };

                mjpegConnection.NewFrame += (x, y) => OnNewFrame(controller, results, y);
                mjpegConnection.Start();
                try
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (ThreadAbortException e)
                {
                    mjpegConnection.Stop();
                }
            }
        }

        private long _startingMachinerySkipFramesCount = 7; //during initialization of web-camera background is black.
        private void OnNewFrame(
            ServerController controller,
            IMessageRouterResults output, 
            NewFrameEventArgs eventargs)
        {
            if (_startingMachinerySkipFramesCount > 0)
            {
                _startingMachinerySkipFramesCount--;
                return;
            }
            using (var bitmap = eventargs.Frame)
            {
/*
Temprarily Disabled
                double totalBrightness = 0;
                var stopWatch = Stopwatch.StartNew();
                for (int width = 0; width < bitmap.Width; width++)
                {
                    for (int height = 0; height < bitmap.Height; height++)
                    {
                        totalBrightness += GetBrightness(bitmap
                            .GetPixel(width, height));
                    }
                }
                var brightnessPerPixel = totalBrightness/(1.0*bitmap.Width*bitmap.Height);
                stopWatch.Stop();

                if (brightnessPerPixel < 1)
                {
                    output.SendBinary(new ServerMessage(
                        controller.Model.Settings.CameraConnection.Reference,
                        "Camera is closed!",
                        string.Format("Brightness : {0}; DateTime : {1}", brightnessPerPixel, DateTime.Now.ToString("g")),
                        ServerMessageImportance.Secutiry));
                }
                else*/
                {
                    var result = controller.Model.MovementDetection.Process(bitmap);

                    if (result != null)
                    {
                        output.SendBinary(new ServerMessage(
                                              controller.Model.Settings.CameraConnection.Reference,
                                              "Movement detection",
                                              result,
                                              ServerMessageImportance.Secutiry));
                    }
                }
            }

            
        }

        #endregion

        #region Private Methods

        private double GetBrightness(Color color)
        {
            return 0.299*color.R + 0.587*color.G + 0.114*color.B;
        }

        #endregion
    }
}
