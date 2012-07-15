using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HDE.IpCamClientServer.Common
{
    /// <summary>
    /// Locates settings file in directory configuration in parent folder, parent of parent folder and so on.
    /// </summary>
    public static class SettingsFileLocator
    {
        #region Constants

        private const string Configuration = "configuration";

        #endregion

        #region Fields

        #endregion

        #region Public Methods

        public static string LocateConfigurationFolder()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string configurationFolder;
            do
            {
                currentFolder = Path.GetDirectoryName(currentFolder);
                if (currentFolder == null)
                {
                    throw new ApplicationException("Application must be executed after configuration via Setup tool, when directory Configuration will be created.");
                }
                configurationFolder = Directory.GetDirectories(currentFolder)
                    .FirstOrDefault(dir=>string.Compare(Path.GetFileName(dir), Configuration, StringComparison.OrdinalIgnoreCase)==0);
            } 
            while (configurationFolder == null);

            return configurationFolder;
        }

        #endregion
    }
}
