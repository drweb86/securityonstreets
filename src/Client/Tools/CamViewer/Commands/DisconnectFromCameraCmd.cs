using HDE.IpCamClientServer.Client.CamViewer.Controller;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class DisconnectFromCameraCmd
    {
        public void DisconnectFromCamera(CamViewerController controller)
        {
            if (controller.Model.MJPEGStream != null)
            {
                controller.Model.MJPEGStream.NewFrame -=
                    controller.Model.OnNewFrame;
                controller.Model.MJPEGStream.Stop();
                controller.Model.MJPEGStream = null;
            }
        }
    }
}
