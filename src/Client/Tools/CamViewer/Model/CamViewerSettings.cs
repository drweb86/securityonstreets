using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using HDE.IpCamClientServer.Common;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Client.CamViewer.Model
{
    public static class CamViewerSettingsHelper
    {
        #region Public Methods

        public static CamViewerSettings Load(ILog log)
        {
            var file = GetViewerSettingsFile();
            var result = new CamViewerSettings();
            if (File.Exists(file))
            {
                log.Info("Load settings");

                var serializer = new XmlSerializer(typeof(CamViewerSettings));
                try
                {
                    using (var settingsFile = File.OpenRead(file))
                    {
                        result = (CamViewerSettings) serializer.Deserialize(settingsFile);
                    }
                }
                catch (Exception unhandledException)
                {
                    log.Error(unhandledException);
                }
            }
            else
            {
                log.Info("File with settings is missing: {0}", file);
            }

            if (result.Connections == null)
            {
                result.Connections = new List<CameraConnection>();
            }

            return result;
        }

        public static void Save(ILog log, CamViewerSettings settings)
        {
            var file = GetViewerSettingsFile();
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var serializer = new XmlSerializer(typeof(CamViewerSettings));
            try
            {
                using (var settingsFile = File.OpenWrite(file))
                {
                    serializer.Serialize(settingsFile, settings);
                }
            }
            catch (Exception unhandledException)
            {
                log.Error(unhandledException);
            }
        }

        #endregion

        #region Private Methdos

        private static string GetViewerSettingsFile()
        {
            return Path.Combine(
                SettingsFileLocator.LocateConfigurationFolder(),
                "HDE.IpCamClientServer.Client.CamViewer.xml");
        }

        #endregion
    }

    [Serializable]
    public class CamViewerSettings
    {
        public List<CameraConnection> Connections { get; set; }
    }
}
