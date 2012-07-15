using System;
using System.Windows.Forms;
using HDE.IpCamClientServer.Client.CamViewer.Commands;
using HDE.IpCamClientServer.Client.CamViewer.Model;
using HDE.IpCamClientServer.Client.CamViewer.View;
using HDE.IpCamClientServer.Common;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Client.CamViewer.Controller
{
    class CamViewerController : IDisposable
    {
        #region Fields

        private readonly UIFactory _uiFactory = new UIFactory();

        #endregion

        #region Properties

        public ILog Log { get; private set; }
        public CamViewerTool Tool { get; private set; }
        public CamViewerModel Model { get; private set; }

        #endregion

        #region Constructors

        public CamViewerController(ILog log, CamViewerTool tool, CamViewerModel model)
        {
            Log = log;
            Tool = tool;
            Model = model;

            _uiFactory.Register<IAddCameraForm, AddCameraForm>();
        }

        #endregion

        #region Commands

        public void ProcessMessage(string subject, params object[] body)
        {
            new ProcessMessageCmd().ProcessMessage(this, subject, body);
        }

        public CameraConnection  EditCameraSettings(CameraConnection connection)
        {
            return new EditCameraSettingsCmd().EditCameraSettings(this, connection);
        }

        public CameraConnection AddCamera()
        {
            return new AddCameraCmd().AddCamera(this);
        }

        public void DeleteCamera(CameraConnection connection)
        {
            new DeleteCameraCmd().DeleteCamera(this, connection);
        }

        public void LoadSettings()
        {
            new LoadSettingsCmd().LoadSettings(this);
        }

        public void SaveSettings()
        {
            new SaveSettingsCmd().SaveSettings(this);
        }

        public void DisconnectFromCamera()
        {
            new DisconnectFromCameraCmd().DisconnectFromCamera(this);
        }

        public void ConnectToCamera(CameraConnection connection)
        {
            new ConnectToCameraCmd().ConnectToCamera(this, connection);
        }

        #endregion

        #region Public Methods

        public void ShowError(string message, params object[] args)
        {
            string messageToShow = message;
            if (args.Length > 0)
            {
                messageToShow = string.Format(message, args);
            }

            MessageBox.Show(messageToShow, "Cam Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public IView CreateView<IView>()
            where IView : IBaseView<CamViewerController>
        {
            var type = _uiFactory.Get(typeof(IView));
            var result = (IBaseView<CamViewerController>)Activator.CreateInstance(type);
            result.SetContext(this);
            return (IView)result;
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            DisconnectFromCamera();
        }

        #endregion
    }
}
