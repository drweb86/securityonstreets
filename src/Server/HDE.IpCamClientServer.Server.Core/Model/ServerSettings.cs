using System;
using HDE.IpCamClientServer.Common;
using MessageRouter.Common;

namespace HDE.IpCamClientServer.Server.Core.Model
{
    [Serializable]
    public class ServerSettings
    {
        #region Properties

        public RhinoQueueConfiguration TargetQueueConfig { get; set; }
        public CameraConnection CameraConnection { get; set; }

        #endregion
    }
}
