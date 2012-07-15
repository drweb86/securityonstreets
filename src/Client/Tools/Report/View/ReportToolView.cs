using System;
using System.Drawing;
using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Report.Controller;
using HDE.IpCamClientServer.Client.Report.Model;
using HDE.IpCamClientServer.Common.Messaging;
using HDE.Platform.AspectOrientedFramework.View;

namespace HDE.IpCamClientServer.Client.Report.View
{
    partial class ReportToolView : UserControl, IToolView
    {
        #region Fields

        private ReportController _controller;

        #endregion
        
        #region Constructors
        
        public ReportToolView()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void TearDown()
        {
            //Detach from Model
            _controller.Model.MessageReceived -= OnMessageReceived;
        }

        public void Init(ReportController controller)
        {
            _controller = controller;

            //attach to Model.
            _controller.Model.MessageReceived += OnMessageReceived;
        }

        public void ApplyChange(string subject, params object[] body)
        {
        }

        #endregion

        #region Private Methods

        private void OnOpenSelectedCameraClick(object sender, EventArgs e)
        {
            var selectedItems = _messagesListView.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            {
                _controller.OpenCameraByReference(((ServerMessage)item.Tag).CameraId);
                break;
            }
        }

        private void OnMessageReceived(ServerMessage message)
        {
            Invoke(new MessageReceived(OnMessageReceivedInternal), message);
        }

        private void OnMessageReceivedInternal(ServerMessage message)
        {
            var listViewItem = new ListViewItem(
                new[]
                    {
                        message.CameraId ?? string.Empty,
                        message.Importance.ToString(),
                        message.Message ?? string.Empty,
                        message.Details ?? string.Empty
                    });
            if (message.Importance == ServerMessageImportance.Secutiry)
            {
                listViewItem.BackColor = Color.Salmon;
                listViewItem.ForeColor = Color.White;
            }
            listViewItem.Tag = message;

            _messagesListView.Items.Insert(0, listViewItem);
        }

        private void OnConnectClick(object sender, EventArgs e)
        {
            _connectToolStripButton.Enabled = false;
            _connectionStatusToolStripLabel.Text = "DISCONNECTED";

            if (_controller.ConnectToServer())
            {
                _connectionStatusToolStripLabel.Text = "CONNECTED";
            }
            else
            {
                _connectionStatusToolStripLabel.Text = "FAILURE";
            }
        }

        private void OnDisconnectClick(object sender, EventArgs e)
        {
            _connectToolStripButton.Enabled = true;
            _controller.DisconnectFromServer();
            _connectionStatusToolStripLabel.Text = "DISCONNECTED";
        }

        #endregion
    }
}
