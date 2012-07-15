using System.Threading;
using HDE.IpCamClientServer.Client.Report.Controller;
using HDE.IpCamClientServer.Client.Report.Model;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class ConnectToServerCmd
    {
        public bool ConnectToServer(ReportController controller)
        {
            controller.DisconnectFromServer();

            controller.Model.ServerConnection = new ImageProcessingRouterSubscriber(controller.Log, controller.Model.Settings.Subscription);
            controller.Model.ServerConnection.ServerMessage +=
                    controller.Model.ServerMessageReceived;

            controller.Model.SubscriberThread = new Thread(controller.Model.ServerConnection.Start);
            controller.Model.SubscriberThread.Start();

            return true;
        }
    }
}
