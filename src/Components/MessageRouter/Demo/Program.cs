using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HDE.IpCamClientServer.Common;
using HDE.Platform.Logging;
using MessageRouter.Common;
using MessageRouter.Server.Core;

namespace MessageRouter.Demo
{
    class Program
    {
        #region Constants

        private const string ExecutableName = "MessageRouter.Demo.exe";
        private const string ConfigurationFile = "MessageRouter.Demo.xml";

        private const string WorkerArgument = "server";

        #endregion

        static void Main(string[] args)
        {
            var serverSettings = ServerSettingsHelper.Load(Path.Combine(SettingsFileLocator.LocateConfigurationFolder(), ConfigurationFile));
            var consoleLog = new ConsoleLog();
            consoleLog.Open();

            if (args.Length == 0)
            {
                Console.WriteLine("Router was started.\nHere you can see messages it forwards to listeners");
                
                StartProcessorsAndSubscribers(serverSettings);

                new DemoRouterServer(consoleLog, serverSettings)
                    .Start();

                Console.ReadLine();
            }
            else if (args[0] == WorkerArgument)
            {
                Console.WriteLine("Worker process.\nType message and press <Enter> to send it to subscribers.");
                using (var results = new MessageRouterResults(consoleLog, serverSettings.Server))
                {
                    while (true)
                    {
                        var text = Console.ReadLine();
                        if (string.IsNullOrEmpty(text))
                        {
                            break;
                        }
                        results.SendText(text);
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} is started.\nHere you can see messages sent by message router.", args[0]);
                var config = serverSettings.Listeners.FirstOrDefault(item => item.Name == args[0]);

                new DemoMessageRouterSubscriber(consoleLog, config)
                    .Start();

            }
        }

        #region Private Methods

        private static void StartProcessorsAndSubscribers(ServerSettings serverSettings)
        {
            Process.Start(ExecutableName, WorkerArgument);
            Process.Start(ExecutableName, WorkerArgument);

            foreach (var listener in serverSettings.Listeners)
            {
                Process.Start(ExecutableName, "\"" + listener.Name + "\"");
            }
        }

        #endregion
    }
}
