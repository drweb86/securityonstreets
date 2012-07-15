using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.View;
using HDE.IpCamClientServer.Common;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class AddCameraCmd
    {
        public CameraConnection AddCamera(CamViewerController controller)
        {
            var form = controller.CreateView<IAddCameraForm>();
            CameraConnection connection = null;
            if (form.Process())
            {
                connection = form.Connection;
                controller.Model.Settings.Connections.Add(connection);
                controller.SaveSettings();
            }

            return connection;
        }
    }
}
