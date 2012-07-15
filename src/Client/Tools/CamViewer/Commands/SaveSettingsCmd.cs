using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.Model;

namespace HDE.IpCamClientServer.Client.CamViewer.Commands
{
    class SaveSettingsCmd
    {
        public void SaveSettings(CamViewerController controller)
        {
            CamViewerSettingsHelper.Save(controller.Log, controller.Model.Settings);
        }
    }
}
