using System;
using HDE.IpCamClientServer.Server.ServerC.Controller;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    interface IDebugView : IDisposable
    {
        void Initialize(string title, ImageSource source);
    }
}