using System.IO;
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
        public IHandler MovementDetection { get; private set; }
        
        #endregion

        #region Contructors

        public ServerModel(IInterceptor interceptor)
        {
            ServerConfigFile = Path.Combine(
                SettingsFileLocator.LocateConfigurationFolder(),
                "HDE.IpCamClientServer.Server.ServerC.xml");

            MovementDetection = new SpagnoloMovementDetector(interceptor);
        }

        #endregion

    }
}
