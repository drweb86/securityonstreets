using System.Text;
using HDE.Platform.Logging;
using MessageRouter.Common;
using Rhino.Queues.Model;

namespace MessageRouter.Demo
{
    class DemoMessageRouterSubscriber : MessageRouterSubscriberBase
    {
        #region Constructors

        public DemoMessageRouterSubscriber(ILog log, RhinoQueueConfiguration subscriptionQueue) : base(log, subscriptionQueue)
        {
        }

        #endregion

        #region Implementation

        protected override void ProcessMessage(Message message)
        {
            Log.Info("[INFO] {0}", Encoding.Unicode.GetString(message.Data));
        }

        #endregion
    }
}