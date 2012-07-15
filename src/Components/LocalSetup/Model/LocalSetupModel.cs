using System.IO;
using System.Reflection;

namespace LocalSetup.Model
{
    class LocalSetupModel
    {
        #region Properties

        public string BinFolder { get; private set; }
        public string RootFolder { get; private set; }
        public string ConfigurationFolder { get; private set; }
        public string Client_ReportToolConfig { get; private set; }
        public string Client_CamViewerToolConfig { get; private set; }

        public string ImageProcessingServer_ServerCConfig { get; private set; }

        public string IpCamEmu_Configuration { get; private set; }

        public string MessageRouter_WpfServerConfig { get; private set; }

        #endregion

        #region Constructors

        public LocalSetupModel()
        {
            BinFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            RootFolder = Path.GetDirectoryName(BinFolder);

            ConfigurationFolder = Path.Combine(
                Path.GetDirectoryName(Path.GetDirectoryName(BinFolder)),
                "configuration");

            Client_ReportToolConfig = Path.Combine(
                ConfigurationFolder, 
                @"HDE.IpCamClientServer.Client.Report.xml");

            Client_CamViewerToolConfig = Path.Combine(
                ConfigurationFolder,
                @"HDE.IpCamClientServer.Client.CamViewer.xml");

            ImageProcessingServer_ServerCConfig = Path.Combine(
                ConfigurationFolder,
                @"HDE.IpCamClientServer.Server.ServerC.xml");

            IpCamEmu_Configuration = Path.Combine(
                RootFolder,
                @"IpCamEmu 1.3\Configuration.xml");

            MessageRouter_WpfServerConfig = Path.Combine(
                ConfigurationFolder,
                @"MessageRouter.Server.WpfServer.xml");
        }

        #endregion
    }
}
