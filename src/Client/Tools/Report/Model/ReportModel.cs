using System.Threading;
using HDE.IpCamClientServer.Common.Messaging;

namespace HDE.IpCamClientServer.Client.Report.Model
{
    delegate void MessageReceived(ServerMessage message);

    class ReportModel
    {
        #region Properties

        public Thread SubscriberThread { get; set; }
        public ImageProcessingRouterSubscriber ServerConnection { get; set; }
        public ReportToolSettings Settings { get; set; }

        #endregion

        #region Public Methods

        public void ServerMessageReceived(object e, ServerMessageArgs args)
        {
            if (MessageReceived != null)
            {
                MessageReceived(args.Message);
            }
        }

        #endregion

        #region Events

        public event MessageReceived MessageReceived;

        #endregion
    }
}
