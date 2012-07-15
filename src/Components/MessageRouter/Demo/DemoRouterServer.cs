using System.Text;
using HDE.Platform.Logging;
using MessageRouter.Server.Core;
using Rhino.Queues.Model;

namespace MessageRouter.Demo
{
    public class DemoRouterServer : RouterServerBase
    {
        #region Constructors

        public DemoRouterServer(ILog log, ServerSettings settings)
            : base(log, settings) 
        {
        }

        #endregion

        #region RouterServer Implementation

        protected override void ProcessMessage(Message message)
        {
            Log.Info("[ROUTING] {0}", Encoding.Unicode.GetString(message.Data));
        }

        #endregion
    }
}