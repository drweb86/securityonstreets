using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Controller;
using HDE.IpCamClientServer.Client.View;

namespace HDE.IpCamClientServer.Client.Commands
{
    class ExecuteCmd
    {
        public void Execute(ClientController controller)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var result = controller.CreateView<IMainFormView>();
            controller.LoadSettings(result);
            Application.Run((Form)result);
        }
    }
}
