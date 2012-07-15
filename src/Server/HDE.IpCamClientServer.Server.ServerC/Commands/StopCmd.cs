using HDE.IpCamClientServer.Server.ServerC.Controller;

namespace HDE.IpCamClientServer.Server.ServerC.Commands
{
    class StopCmd
    {
        #region Public Methods

        public void Stop(ServerController controller)
        {
            if (controller.Model.WorkerThread != null)
            {
                controller.Model.WorkerThread.Abort();
                controller.Model.WorkerThread = null;
            }
        }

        #endregion
    }
}
