using System;
using System.Timers;
using HDE.IpCamClientServer.Common.Messaging;

namespace HDE.IpCamClientServer.Server.Core
{
    public class ServerWorker : IDisposable
    {
        #region StubLogic

        private readonly Timer _timer = new Timer();

        public ServerWorker()
        {
            _timer.Interval = 1000;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private long test;
        void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (ServerMessage != null)
            {
                test++;
                ServerMessage(this, new ServerMessageArgs(new ServerMessage(
                    "Cam-1880",
                    "Hi " + test,
                    "Some details",
                    ServerMessageImportance.Secutiry)));
            }
        }

        #endregion

        public EventHandler<ServerMessageArgs> ServerMessage;

        public void Dispose()
        {
        }
    }
}
