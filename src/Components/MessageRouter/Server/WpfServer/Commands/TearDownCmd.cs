using MessageRouter.Server.WpfServer.Controller;

namespace MessageRouter.Server.WpfServer.Commands
{
    class TearDownCmd
    {
        public void TearDown(WpfServerController controller)
        {
            try
            {
                if (controller.Model.WorkerThread != null)
                {
                    controller.Model.WorkerThread.Abort();
                }
                controller.Model.Router.Dispose();
            }
            catch
            {
            }
        }
    }
}
