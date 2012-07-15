using System.Net;
using Rhino.Queues;

namespace MessageRouter.Common
{
    public class RhinoHelper
    {
        public static QueueManager OpenServer(RhinoQueueConfiguration queueConfiguration)
        {
            var queueManager = new QueueManager(
                new IPEndPoint(
                    DnsHelper.GetAddresses(queueConfiguration.Host)[0],
                    queueConfiguration.Port),
                queueConfiguration.Name + " Queues");
            queueManager.CreateQueues(queueConfiguration.QueueName);

            return queueManager;
        }
    }
}