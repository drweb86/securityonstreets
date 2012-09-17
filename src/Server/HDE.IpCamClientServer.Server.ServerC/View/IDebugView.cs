using System;
using System.Drawing;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    internal interface IDebugView : IDisposable
    {
        void Initialize(string title);
        void Update(Image image);
    }
}