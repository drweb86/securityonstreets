using System.IO;
using System.Reflection;
using System.Threading;
using HDE.IpCamClientServer.Common;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using HDE.IpCamClientServer.Server.Core.Model;

namespace HDE.IpCamClientServer.Server.ServerC.Model
{
    class ServerModel
    {
        #region Properties

        public ServerSettings Settings { get; set; }
        public string ServerConfigFile { get; private set; }
        public Thread WorkerThread { get; set; }
        public MovementHandler MovementDetection { get; private set; }

        #endregion

        #region Contructors

        public ServerModel()
        {
            ServerConfigFile = Path.Combine(
                SettingsFileLocator.LocateConfigurationFolder(),
                "HDE.IpCamClientServer.Server.ServerC.xml");

            MovementDetection = new MovementHandler();
        }

        #endregion

    }
}
