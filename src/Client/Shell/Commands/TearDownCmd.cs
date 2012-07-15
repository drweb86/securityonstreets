using HDE.IpCamClientServer.Client.Controller;

namespace HDE.IpCamClientServer.Client.Commands
{
    class TearDownCmd
    {
        public void TearDown(ClientController controller)
        {
            foreach (var tool in controller.Model.Tools)
            {
                tool.Dispose();
            }
        }
    }
}
