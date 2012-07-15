using System;

namespace MessageRouter.Common
{
    public interface IMessageRouterSubscriber:IDisposable
    {
        void Start();
        bool RequestStopping { get; set; }
    }
}
