using HDE.IpCamClientServer.Client.Report.Controller;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class DisconnectFromServerCmd
    {
        public void DisconnectFromServer(ReportController controller)
        {
            if (controller.Model.SubscriberThread != null)
            {
                controller.Model.SubscriberThread.Abort();
                controller.Model.SubscriberThread = null;
            }

            if (controller.Model.ServerConnection != null)
            {
                controller.Model.ServerConnection.ServerMessage -=
                    controller.Model.ServerMessageReceived;
                controller.Model.ServerConnection.RequestStopping = true;
                controller.Model.ServerConnection.Dispose();
                controller.Model.ServerConnection = null;
            }
        }
    }
}
