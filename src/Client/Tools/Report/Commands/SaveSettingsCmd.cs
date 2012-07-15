using HDE.IpCamClientServer.Client.Report.Controller;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class SaveSettingsCmd
    {
        public void SaveSettings(ReportController controller)
        {
            ReportToolSettingsHelper.Save(controller.Log, controller.Model.Settings);
        }
    }
}
