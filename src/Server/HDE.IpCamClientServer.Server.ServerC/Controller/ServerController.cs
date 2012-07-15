﻿using System;
using System.IO;
using HDE.IpCamClientServer.Server.ServerC.Commands;
using HDE.IpCamClientServer.Server.ServerC.Model;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class ServerController: IDisposable
    {
        #region Properties

        public ServerModel Model { get; private set; }
        public ILog Log { get; private set; }

        #endregion

        #region Constructors

        public ServerController()
        {
            Model = new ServerModel();

            Log = new QueueLog(
                new ConsoleLog(),
                new SimpleFileLog(
                    Path.Combine(
                        Path.GetTempPath(),
                        "HDE.IpCamClientServer.Server.ServerC")));

            Log.Open();

            AppDomain.CurrentDomain.UnhandledException += (s, e) => ReportIssue(e.ExceptionObject);
        }

        #endregion

        #region TearDown

        public void Dispose()
        {
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