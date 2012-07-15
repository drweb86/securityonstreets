using HDE.IpCamClientServer.Client.Report.Controller;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class LoadSettingsCmd
    {
        public void LoadSettings(ReportController controller)
        {
            var result = ReportToolSettingsHelper.Load(controller.Log);
            if (result != null)
            {
                controller.Model.Settings = result;
            }
        }
    }
}
