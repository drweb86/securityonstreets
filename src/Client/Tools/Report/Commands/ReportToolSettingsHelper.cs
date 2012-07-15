using System.IO;
using System.Reflection;
using HDE.IpCamClientServer.Client.Report.Model;
using HDE.IpCamClientServer.Common;
using HDE.Platform.Logging;
using HDE.Platform.Serialization;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    static class ReportToolSettingsHelper
    {
        #region Constants

        private const string _settingsFileName = "HDE.IpCamClientServer.Client.Report.xml";

        #endregion

        #region Public Methods

        public static ReportToolSettings Load(ILog log)
        {
            var settingsFile = GetToolSettingsFile();
            log.Info("Loading Report tool settings...");
            return SerializerHelper.Load<ReportToolSettings>(settingsFile);
        }

        public static void Save(ILog log, ReportToolSettings settings)
        {
            var settingsFile = GetToolSettingsFile();
            SerializerHelper.Save(settings, settingsFile);
        }

        #endregion

        #region Private Methods

        private static string GetToolSettingsFile()
        {
            return Path.Combine(
                SettingsFileLocator.LocateConfigurationFolder(),
                _settingsFileName);
        }

        #endregion
    }
}