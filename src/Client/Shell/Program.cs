using System;
using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Controller;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var controller = new ClientController())
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportIssue(controller.Log, e.ExceptionObject);
                Application.ThreadException += (s, e) => ReportIssue(controller.Log, e.Exception);

                try
                {
                    controller.Execute();
                }
                catch (Exception unhandledException)
                {
                    controller.Log.Error(unhandledException);
                }
            }
        }

        private static void ReportIssue(ILog log, object unhandledException)
        {
            var typedExc = unhandledException as Exception;
            if (typedExc != null)
            {
                log.Error(typedExc);
                MessageBox.Show(typedExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
