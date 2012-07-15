using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.View;
using HDE.IpCamClientServer.Common;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class EditCameraSettingsCmd
    {
        public CameraConnection EditCameraSettings(CamViewerController controller, CameraConnection cameraSettings)
        {
            var form = controller.CreateView<IAddCameraForm>();
            CameraConnection connection = null;
            form.Connection = cameraSettings;
            if (form.Process())
            {
                connection = form.Connection;
                controller.Model.Settings.Connections.Remove(cameraSettings);
                controller.Model.Settings.Connections.Add(connection);
                controller.SaveSettings();
            }

            return connection;
        }
    }
}
