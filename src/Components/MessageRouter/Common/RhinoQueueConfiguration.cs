using System;

namespace MessageRouter.Common
{
    /// <summary>
    /// Configuration.
    /// </summary>
    [Serializable]
    public class RhinoQueueConfiguration
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
    }
}
