using System;

namespace HDE.IpCamClientServer.Common
{
    [Serializable]
    public class CameraConnection
    {
#warning: add camera type, now its for MJPEG only.

        public string Uri { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
    }
}
