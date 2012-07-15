using System;
using System.Windows.Forms;
using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.Common.View;
using HDE.IpCamClientServer.Common;
using HDE.Platform.AspectOrientedFramework;

namespace HDE.IpCamClientServer.Client.CamViewer.View
{
    interface IAddCameraForm : IBaseView<CamViewerController>
    {
        CameraConnection Connection { get; set; }
    }

    partial class AddCameraForm : BaseForm, IAddCameraForm
    {
        #region Fields

        private CamViewerController _controller;

        #endregion
        
        #region Contructors

        public AddCameraForm()
        {
            InitializeComponent();
        }

        #endregion

        public void SetContext(CamViewerController context)
        {
            _controller = context;
        }

        public bool Process()
        {
            return ShowDialog() == DialogResult.OK;
        }

        public CameraConnection Connection
        {
            get 
            { 
                return new CameraConnection
                    {
                        Description = _descriptionTextBox.Text,
                        Password = _passwordTextBox.Text,
                        Reference = _referenceTextBox.Text,
                        Title = _titleTextBox.Text,
                        Uri = _uriTextBox.Text,
                        User = _userTextBox.Text
                    }; 
            }
            set
            {
                _descriptionTextBox.Text = value.Description;
                _passwordTextBox.Text = value.Password;
                _referenceTextBox.Text = value.Reference;
                _titleTextBox.Text = value.Title;
                _uriTextBox.Text = value.Uri;
                _userTextBox.Text = value.User;
            }
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_titleTextBox.Text))
            {
                _controller.ShowError("Title is empty!");
                return;
            }

            if (string.IsNullOrWhiteSpace(_uriTextBox.Text))
            {
                _controller.ShowError("Uri is empty!");
                return;
            }

            if (string.IsNullOrWhiteSpace(_passwordTextBox.Text))
            {
                _controller.ShowError("User is empty!");
                return;
            }

            if (string.IsNullOrWhiteSpace(_passwordTextBox.Text))
            {
                _controller.ShowError("Password is empty!");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
