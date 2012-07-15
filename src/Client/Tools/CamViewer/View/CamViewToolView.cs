using System.Windows.Forms;
using AForge.Video;
using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Common;
using HDE.Platform.AspectOrientedFramework.View;

namespace HDE.IpCamClientServer.Client.CamViewer.View
{
    partial class CamViewToolView : UserControl, IToolView
    {
        #region Fields

        private CamViewerController _controller;

        #endregion

        #region Contructors

        public CamViewToolView()
        {
            InitializeComponent();
            _camerasListBox.ValueMember = "Title";// CameraConnection.Title
        }

        #endregion

        #region IToolView Implementation

        public void ApplyChange(string subject, params object[] body)
        {
            switch (subject)
            {
                case ViewChanges.SelectCamera:
                    var cam = body[0];
                    _camerasListBox.SelectedItem = cam;
                    break;
            }
        }

        #endregion

        public void TearDown()
        {
            _controller.Model.NewFrame -= OnFrameReceived;
        }

        public void Init(CamViewerController controller)
        {
            _controller = controller;

            foreach (var cameraCnn in _controller.Model.Settings.Connections)
            {
                AddCameraToUi(cameraCnn);
            }

            _controller.Model.NewFrame += OnFrameReceived;
        }

        private void OnAddCamera(object sender, System.EventArgs e)
        {
            var camera = _controller.AddCamera();
            if (camera != null)
            {
                AddCameraToUi(camera);
                _camerasListBox.SelectedItem = camera;
            }
        }

        private void AddCameraToUi(CameraConnection camera)
        {
            _camerasListBox.Items.Add(camera);
        }

        private void DeleteCameraFromUi(CameraConnection camera)
        {
            _camerasListBox.Items.Remove(camera);
        }

        private void OnDeleteSelectedCamera(object sender, System.EventArgs e)
        {
            if (_camerasListBox.SelectedItem != null)
            {
                var selectedCamera = (CameraConnection)_camerasListBox.SelectedItem;
                _controller.DeleteCamera(selectedCamera);
                DeleteCameraFromUi(selectedCamera);
            }
        }

        private void OnConnectToSelectedCam(object sender, System.EventArgs e)
        {
            if (_camerasListBox.SelectedItem != null)
            {
                _controller.ConnectToCamera((CameraConnection)_camerasListBox.SelectedItem);
            }
        }

        void OnFrameReceived(object sender, NewFrameEventArgs eventArgs)
        {
            _viewPictureBox.CreateGraphics().DrawImageUnscaled(eventArgs.Frame, _viewPictureBox.Top, _viewPictureBox.Left);
        }

        private void OnDisconnectClick(object sender, System.EventArgs e)
        {
            _controller.DisconnectFromCamera();
        }

        private void OnEditSelectedCameraSettings(object sender, System.EventArgs e)
        {
            if (_camerasListBox.SelectedItem != null)
            {
                var newSettings = _controller.EditCameraSettings((CameraConnection)_camerasListBox.SelectedItem);
                if (newSettings != null)
                {
                    DeleteCameraFromUi((CameraConnection) _camerasListBox.SelectedItem);
                    AddCameraToUi(newSettings);
                    _camerasListBox.SelectedItem = newSettings;
                }
            }
        }

        private void OnSelectedCameraChanged(object sender, System.EventArgs e)
        {
            if (_camerasListBox.SelectedItem != null)
            {
                _selectedCameraInfoLabel.Text = ((CameraConnection) _camerasListBox.SelectedItem).Description;
            }
            else
            {
                _selectedCameraInfoLabel.Text = string.Empty;
            }
        }
    }
}
