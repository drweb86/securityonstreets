using System;

namespace HDE.IpCamClientServer.Common.Messaging
{
    [Serializable]
    public class ServerMessage
    {
        #region Properties

        public string CameraId { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ServerMessageImportance Importance { get; set; }

        #endregion

        #region Contructors

        public ServerMessage()
        {
            // Serialization
        }

        public ServerMessage(
            string cameraId,
            string message,
            string details,
            ServerMessageImportance importance)
        {
            CameraId = cameraId;
            Message = message;
            Details = details;
            Importance = importance;
        }

        #endregion
    }
}
