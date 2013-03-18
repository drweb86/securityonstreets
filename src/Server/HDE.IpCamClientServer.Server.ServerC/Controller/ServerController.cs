using System;
using System.Configuration;
using System.IO;
using HDE.IpCamClientServer.Server.ServerC.Commands;
using HDE.IpCamClientServer.Server.ServerC.Model;
using HDE.IpCamClientServer.Server.ServerC.View;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class ServerController: IDisposable
    {
        #region Fields

        private DebugInterceptor _interceptor;

        #endregion

        #region Properties

        public ServerModel Model { get; private set; }
        public ILog Log { get; private set; }
        public OperationMode OperationMode { get; private set; }
        public string AppName { get { return "HDE.IpCamClientServer.Server.ServerC"; } }

        #endregion

        #region Constructors

        public ServerController()
        {
            Log = new QueueLog(
                new ConsoleLog(),
                new SimpleFileLog(
                    Path.Combine(
                        Path.GetTempPath(),
                        AppName)));

            Log.Open();

            AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportIssue(e.ExceptionObject);
            OperationMode = (OperationMode)Enum.Parse(typeof(OperationMode), ConfigurationManager.AppSettings["OperationMode"], true);

            if (OperationMode == OperationMode.Debug)
            {
                _interceptor = new DebugInterceptor();
            }
            Model = new ServerModel(_interceptor);
        }

        #endregion

        #region TearDown

        public void Dispose()
        {
            if (_interceptor != null)
            {
                _interceptor.Dispose();
                _interceptor = null;
            }

            if (Log != null)
            {
                Log.Close();
                Log = null;
            }
        }

        #endregion

        #region Commands

        public void Start()
        {
            new StartCmd().Start(this);
        }

        public void Stop()
        {
            new StopCmd().Stop(this);
        }

        public void LoadSettings()
        {
            new LoadSettingsCmd().LoadSettings(this);
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
    }
}
