using System.Diagnostics;
using MessageRouter.Server.WpfServer.Controller;

namespace MessageRouter.Server.WpfServer.Commands
{
    class OpenLogsFolderCmd
    {
        public void OpenLogsFolder(WpfServerController controller)
        {
            Process.Start("explorer.exe", controller.Model.LogsFolder);
        }
    }
}
