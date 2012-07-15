using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Common;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class DeleteCameraCmd
    {
        public void DeleteCamera(CamViewerController controller, CameraConnection connection)
        {
            controller.Model.Settings.Connections.Remove(connection);

            controller.SaveSettings();
        }
    }
}
