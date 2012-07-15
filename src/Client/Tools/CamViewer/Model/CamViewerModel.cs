using AForge.Video;

namespace HDE.IpCamClientServer.Client.CamViewer.Model
{
    class CamViewerModel
    {
        #region Properties

        public CamViewerSettings Settings { get; set; }
        public MJPEGStream MJPEGStream { get; set; }

        #endregion

        #region Events

        public event NewFrameEventHandler NewFrame;

        #endregion

        #region Constructors

        public CamViewerModel()
        {
            Settings = new CamViewerSettings();
        }

        #endregion

        #region Public Methods

        public void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (NewFrame != null)
            {
                NewFrame(sender, eventArgs);
            }
        }

        #endregion
    }
}
