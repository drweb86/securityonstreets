using System;
using MessageRouter.Common;

namespace HDE.IpCamClientServer.Client.Report.Model
{
    [Serializable]
    public class ReportToolSettings
    {
        public RhinoQueueConfiguration Subscription { get; set; }
    }
}