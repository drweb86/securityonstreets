using System;

namespace MessageRouter.Server.Core
{
    public interface IRouterServer : IDisposable
    {
        void Start();
    }
}