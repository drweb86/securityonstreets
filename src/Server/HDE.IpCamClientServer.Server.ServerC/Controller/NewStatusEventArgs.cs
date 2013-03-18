using System;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class NewStatusEventArgs : EventArgs
    {
        #region Properties

        public string Status { get; private set; }

        #endregion

        #region Constructors

        public NewStatusEventArgs(string status)
        {
            Status = status;
        }

        #endregion
    }
}