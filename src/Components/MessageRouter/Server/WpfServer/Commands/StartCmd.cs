using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using HDE.IpCamClientServer.Common;
using MessageRouter.Server.Core;
using MessageRouter.Server.WpfServer.Controller;

namespace MessageRouter.Server.WpfServer.Commands
{
    class StartCmd
    {
        public bool Start(WpfServerController controller)
        {
            try
            {
                var localPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                controller.Model.ServerSettings = ServerSettingsHelper.Load(Path.Combine(SettingsFileLocator.LocateConfigurationFolder(), "MessageRouter.Server.WpfServer.xml"));

                controller.Model.Router = new WpfRouterServer(controller.Log, controller.Model.ServerSettings);
                controller.Model.WorkerThread = new Thread(controller.Model.Router.Start);
                controller.Model.WorkerThread.Start();

                return true;
            }
            catch (Exception e)
            {
                controller.Log.Error(e);
                MessageBox.Show(e.ToString(), "Message Router", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
