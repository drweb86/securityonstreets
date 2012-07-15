namespace LocalSetup.Model
{
    class LocalSetupSettings
    {
        #region Properties

        public string UserPassword   { get; private set; }
        public uint MessageRouterInputPort { get; private set; }
        public uint ClientInputPort { get; private set; }
        public uint IpCameraEmulatorPort { get; private set; }
        public string IpCamId { get; private set; }
        public string IpCamName { get; private set; }

        #endregion

        #region Constructors

        public LocalSetupSettings(
            string userPassword,
            uint messageRouterInputPort,
            uint clientInputPort,
            uint ipCameraEmulatorPort,
            string ipCamId,
            string ipCamName)
        {
            UserPassword = userPassword;
            MessageRouterInputPort = messageRouterInputPort;
            ClientInputPort = clientInputPort;
            IpCameraEmulatorPort = ipCameraEmulatorPort;
            IpCamId = ipCamId;
            IpCamName = ipCamName;
        }

        #endregion
    }
}
