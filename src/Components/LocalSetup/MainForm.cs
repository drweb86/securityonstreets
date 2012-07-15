using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LocalSetup.Controller;
using LocalSetup.Model;

namespace LocalSetup
{
    public partial class MainForm : Form
    {
        #region Fields

        private LocalSetupController _controller;

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void OnPrepareButtonClick(object sender, EventArgs e)
        {
            _controller.Setup(GetSettings());
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _controller = new LocalSetupController();
            _userPasswordLabel.Text = string.Format("Enter Windows password of {0}:", Environment.UserName);
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            if (_controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }
        }

        private LocalSetupSettings GetSettings()
        {
            return new LocalSetupSettings(
                _userPasswordTextBox.Text,
                uint.Parse(_messageRouterInputPortTextBox.Text),
                uint.Parse(_clientInputPortTextBox.Text),
                uint.Parse(_ipCameraEmulatorPortTextBox.Text),
                _ipCamIdTextBox.Text,
                _ipCamNameTextBox.Text);
        }

        #endregion
    }
}
