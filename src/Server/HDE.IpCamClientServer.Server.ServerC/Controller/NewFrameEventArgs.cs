using System;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class NewFrameEventArgs : EventArgs
    {
        #region Properties

        public string Key { get; private set; }
        public byte[] Frame { get; private set; }

        #endregion

        #region Constructors

        public NewFrameEventArgs(string key, byte[] frame)
        {
            Key = key;
            Frame = frame;
        }

        #endregion
    }
}