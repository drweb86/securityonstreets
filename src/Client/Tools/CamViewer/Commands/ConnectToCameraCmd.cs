using AForge.Video;
using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.View;
using HDE.IpCamClientServer.Common;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class ConnectToCameraCmd
    {
        public void ConnectToCamera(CamViewerController controller, CameraConnection connection)
        {
            controller.DisconnectFromCamera();

            controller.Log.Info("Connecting to {0}", connection.Title);
            controller.Model.MJPEGStream = new MJPEGStream
                             {
                                 Source = connection.Uri,
                                 Login = connection.User, 
                                 Password = connection.Password, 
                             };
            controller.Model.MJPEGStream.NewFrame +=
                controller.Model.OnNewFrame;
            controller.Model.MJPEGStream.Start();

            controller.Tool.ApplyChange(ViewChanges.SelectCamera, connection);
        }
    }
}
