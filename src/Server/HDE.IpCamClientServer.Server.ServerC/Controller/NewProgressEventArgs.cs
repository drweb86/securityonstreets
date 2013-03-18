using System;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class NewProgressEventArgs : EventArgs
    {
        #region Properties

        public int Current { get; private set; }
        public int Total { get; private set; }

        #endregion

        #region Constructors

        public NewProgressEventArgs(int current, int total)
        {
            Current = current;
            Total = total;
        }

        #endregion
    }
}