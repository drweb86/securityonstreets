using System;

namespace MessageRouter.Common
{
    public interface IMessageRouterResults : IDisposable
    {
        RhinoQueueConfiguration DestinationConfiguration { get; }

        void SendText(string text);
        void SendBinary(byte[] data);
        void SendBinary(object data);
    }
}