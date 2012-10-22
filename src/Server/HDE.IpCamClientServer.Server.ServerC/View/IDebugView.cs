using System;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    internal interface IDebugView : IDisposable
    {
        void Initialize(string title);
        void Update(byte[] image);
    }
}