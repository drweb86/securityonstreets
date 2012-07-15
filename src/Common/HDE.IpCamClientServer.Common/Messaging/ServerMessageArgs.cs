using System;

namespace HDE.IpCamClientServer.Common.Messaging
{
    public class ServerMessageArgs : EventArgs
    {
        #region Properties

        public ServerMessage Message { get; private set; }

        #endregion

        #region Contructors

        public ServerMessageArgs(ServerMessage message)
        {
            Message = message;
        }

        #endregion
    }
}