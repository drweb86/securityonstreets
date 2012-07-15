using HDE.IpCamClientServer.Common;
using HDE.IpCamClientServer.Server.Core.Model;
using HDE.IpCamClientServer.Server.ServerC.Controller;
using HDE.Platform.Serialization;

namespace HDE.IpCamClientServer.Server.ServerC.Commands
{
    class LoadSettingsCmd
    {
        public void LoadSettings(ServerController controller)
        {
            controller.Model.Settings = SerializerHelper.Load<ServerSettings>(controller.Model.ServerConfigFile);
        }
    }
}
