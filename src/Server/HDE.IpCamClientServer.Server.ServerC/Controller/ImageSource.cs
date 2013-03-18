using System;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class ImageSource
    {
        public EventHandler<NewStatusEventArgs> NewStatusReceived;
        public EventHandler<NewProgressEventArgs> NewProgressReceived;

        public EventHandler<NewFrameEventArgs> NewFrameReceived;
        public EventHandler<EventArgs> DisposeRequested;
    }
}