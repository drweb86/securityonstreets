using System;

namespace HDE.IpCamClientServer.Common.AspectOrientedFramework.Services
{
    public interface IMessagePump
    {
        void SendMessage(string to, string subject, params object[] body);
        event EventHandler<MessagePumpArgs> OnMessageReceived;
    }
}