using System;
using System.Windows;
using HDE.Platform.Logging;
using MessageRouter.Server.WpfServer.Commands;
using MessageRouter.Server.WpfServer.Model;

namespace MessageRouter.Server.WpfServer.Controller
{
    class WpfServerController : IDisposable
    {
        #region Properties

        public WpfServerModel Model { get; private set; }
        public ILog Log { get; private set; }

        #endregion

        #region Constructors

        public WpfServerController()
        {
            Model = new WpfServerModel();
            Log = new SimpleFileLog(Model.LogsFolder);
            Log.Open();

            AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportIssue(e.ExceptionObject);
            Application.Current.DispatcherUnhandledException += (s, e) => ReportIssue(e.Exception);
        }

        #endregion

        #region Private Methods

        private void ReportIssue(object unhandledException)
        {
            var typedExc = unhandledException as Exception;
            if (typedExc != null)
            {
                Log.Error(typedExc);
            }
        }

        #endregion

        #region Commands

        public void OpenLogsFolder()
        {
            new OpenLogsFolderCmd().OpenLogsFolder(this);
        }

        public void TearDown()
        {
            new TearDownCmd().TearDown(this);
        }

        public bool Start()
        {
            return new StartCmd().Start(this);
        }

        #endregion

        #region Tear Down

        public void Dispose()
        {
            if (Log != null)
            {
                Log.Close();
                Log = null;
            }
        }

        #endregion
    }
}
