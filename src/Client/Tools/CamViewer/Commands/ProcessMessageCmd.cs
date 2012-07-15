using System.Linq;
using HDE.IpCamClientServer.Client.CamViewer.Controller;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class ProcessMessageCmd
    {
        public void ProcessMessage(CamViewerController controller, string subject, params object[] body)
        {
            switch (subject)
            {
                case "OpenCameraByReference":
                    ConnectToCamera(controller, body);
                    break;
            }
        }

        #region Implementation

        private void ConnectToCamera(CamViewerController controller, params object[] args)
        {
            var camRef = (string)args[0];
            var targetCam = controller.Model.Settings.Connections.FirstOrDefault(cnn => cnn.Reference == camRef);
            if (targetCam == null)
            {
                controller.ShowError("Camera with reference {0} is missing.", camRef);
            }
            else
            {
                controller.ConnectToCamera(targetCam);
            }
        }

        #endregion
    }
}
