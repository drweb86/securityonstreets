using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.Model;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class LoadSettingsCmd
    {
        public void LoadSettings(CamViewerController controller)
        {
            controller.Model.Settings = CamViewerSettingsHelper.Load(controller.Log);
        }
    }
}
