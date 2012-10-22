using System.Drawing;
using System.Threading;
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
            if (controller.OperationMode == OperationMode.Debug)
            {
                controller.Model.WorkerThread.SetApartmentState(ApartmentState.STA);
            }
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

        private void OnNewFrame(
            ServerController controller,
            IMessageRouterResults output, 
            NewFrameEventArgs eventargs)
        {
            using (var bitmap = eventargs.Frame)
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

        #endregion
    }
}
