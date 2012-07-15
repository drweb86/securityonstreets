using System.IO;
using System.Threading;
using MessageRouter.Server.Core;

namespace MessageRouter.Server.WpfServer.Model
{
    class WpfServerModel
    {
        #region Properties

        public ServerSettings ServerSettings { get; set; }
        public WpfRouterServer Router { get; set; }
        public Thread WorkerThread { get; set; }
        public string LogsFolder { get; set; }

        #endregion

        #region Constructors

        public WpfServerModel()
        {
            LogsFolder = Path.Combine(
                Path.GetTempPath(),
                @"HDE\MessageRouter.Server.WpfServer");
        }

        #endregion
    }
}
