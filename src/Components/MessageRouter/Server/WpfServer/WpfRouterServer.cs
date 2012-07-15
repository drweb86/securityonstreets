using HDE.Platform.Logging;
using MessageRouter.Server.Core;
using Rhino.Queues.Model;

namespace MessageRouter.Server.WpfServer
{
    class WpfRouterServer : RouterServerBase
    {
        #region Constructors

        public WpfRouterServer(ILog log, ServerSettings settings) : base(log, settings)
        {
        }

        #endregion

        #region Implementation

        protected override void ProcessMessage(Message message)
        {
        }

        #endregion
    }
}
