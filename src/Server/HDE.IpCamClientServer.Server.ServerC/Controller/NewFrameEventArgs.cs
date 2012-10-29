using System;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class NewFrameEventArgs : EventArgs
    {
        #region Properties

        public byte[] Frame { get; private set; }

        #endregion

        #region Constructors

        public NewFrameEventArgs(byte[] frame)
        {
            Frame = frame;
        }

        #endregion
    }
}