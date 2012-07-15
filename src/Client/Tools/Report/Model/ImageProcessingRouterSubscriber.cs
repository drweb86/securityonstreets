using System;
using HDE.IpCamClientServer.Common;
using HDE.IpCamClientServer.Common.Messaging;
using HDE.Platform.Logging;
using HDE.Platform.Serialization;
using MessageRouter.Common;
using Rhino.Queues.Model;

namespace HDE.IpCamClientServer.Client.Report.Model
{
    class ImageProcessingRouterSubscriber : MessageRouterSubscriberBase
    {
        #region Events

        public EventHandler<ServerMessageArgs> ServerMessage;

        #endregion

        #region Constructors

        public ImageProcessingRouterSubscriber(ILog log, RhinoQueueConfiguration subscriptionQueue)
            : base(log, subscriptionQueue)
        {
        }

        #endregion

        #region Implementation

        protected override void ProcessMessage(Message message)
        {
            var result = SerializerHelper.DeserializeBinary<ServerMessage>(message.Data);
            if (ServerMessage != null)
            {
                ServerMessage(this, new ServerMessageArgs(result));
            }
        }

        #endregion
    }
}
