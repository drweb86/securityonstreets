using HDE.IpCamClientServer.Client.Common.View;

namespace HDE.IpCamClientServer.Client.Report.View
{
    partial class ReportToolView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportToolView));
            this._toolMainStrip = new System.Windows.Forms.ToolStrip();
            this._serverToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._connectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._disconnectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._statusToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this._connectionStatusToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._viewToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this._openCameraToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._messagesListView = new HDE.IpCamClientServer.Client.Common.View.ListViewNF();
            this._cameraIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._importanceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._messageColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._descriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._listViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openSelectedCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolMainStrip.SuspendLayout();
            this._listViewContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolMainStrip
            // 
            this._toolMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._serverToolStripLabel,
            this.toolStripSeparator1,
            this._connectToolStripButton,
            this._disconnectToolStripButton,
            this.toolStripSeparator3,
            this._statusToolStripLabel,
            this._connectionStatusToolStripLabel,
            this.toolStripSeparator4,
            this._viewToolStripLabel,
            this._openCameraToolStripButton});
            this._toolMainStrip.Location = new System.Drawing.Point(0, 0);
            this._toolMainStrip.Name = "_toolMainStrip";
            this._toolMainStrip.Size = new System.Drawing.Size(677, 25);
            this._toolMainStrip.TabIndex = 0;
            this._toolMainStrip.Text = "toolStrip1";
            // 
            // _serverToolStripLabel
            // 
            this._serverToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._serverToolStripLabel.Name = "_serverToolStripLabel";
            this._serverToolStripLabel.Size = new System.Drawing.Size(50, 22);
            this._serverToolStripLabel.Text = "SERVER";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _connectToolStripButton
            // 
            this._connectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._connectToolStripButton.Image = global::HDE.IpCamClientServer.Client.Report.Properties.Resources.Start;
            this._connectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._connectToolStripButton.Name = "_connectToolStripButton";
            this._connectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._connectToolStripButton.Text = "Connect";
            this._connectToolStripButton.Click += new System.EventHandler(this.OnConnectClick);
            // 
            // _disconnectToolStripButton
            // 
            this._disconnectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._disconnectToolStripButton.Image = global::HDE.IpCamClientServer.Client.Report.Properties.Resources.Stop;
            this._disconnectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._disconnectToolStripButton.Name = "_disconnectToolStripButton";
            this._disconnectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._disconnectToolStripButton.Text = "Disconnect";
            this._disconnectToolStripButton.Click += new System.EventHandler(this.OnDisconnectClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // _statusToolStripLabel
            // 
            this._statusToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._statusToolStripLabel.Name = "_statusToolStripLabel";
            this._statusToolStripLabel.Size = new System.Drawing.Size(52, 22);
            this._statusToolStripLabel.Text = "STATUS";
            // 
            // _connectionStatusToolStripLabel
            // 
            this._connectionStatusToolStripLabel.Name = "_connectionStatusToolStripLabel";
            this._connectionStatusToolStripLabel.Size = new System.Drawing.Size(94, 22);
            this._connectionStatusToolStripLabel.Text = "DISCONNECTED";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // _viewToolStripLabel
            // 
            this._viewToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._viewToolStripLabel.Name = "_viewToolStripLabel";
            this._viewToolStripLabel.Size = new System.Drawing.Size(37, 22);
            this._viewToolStripLabel.Text = "VIEW";
            // 
            // _openCameraToolStripButton
            // 
            this._openCameraToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._openCameraToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_openCameraToolStripButton.Image")));
            this._openCameraToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openCameraToolStripButton.Name = "_openCameraToolStripButton";
            this._openCameraToolStripButton.Size = new System.Drawing.Size(131, 22);
            this._openCameraToolStripButton.Text = "Open Selected Camera";
            this._openCameraToolStripButton.Click += new System.EventHandler(this.OnOpenSelectedCameraClick);
            // 
            // _messagesListView
            // 
            this._messagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._cameraIdColumnHeader,
            this._importanceColumnHeader,
            this._messageColumnHeader,
            this._descriptionColumnHeader});
            this._messagesListView.ContextMenuStrip = this._listViewContextMenuStrip;
            this._messagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._messagesListView.FullRowSelect = true;
            this._messagesListView.Location = new System.Drawing.Point(0, 25);
            this._messagesListView.MultiSelect = false;
            this._messagesListView.Name = "_messagesListView";
            this._messagesListView.Size = new System.Drawing.Size(677, 194);
            this._messagesListView.TabIndex = 1;
            this._messagesListView.UseCompatibleStateImageBehavior = false;
            this._messagesListView.View = System.Windows.Forms.View.Details;
            this._messagesListView.DoubleClick += new System.EventHandler(this.OnOpenSelectedCameraClick);
            // 
            // _cameraIdColumnHeader
            // 
            this._cameraIdColumnHeader.Text = "Camera ID";
            this._cameraIdColumnHeader.Width = 77;
            // 
            // _importanceColumnHeader
            // 
            this._importanceColumnHeader.Text = "Importance";
            this._importanceColumnHeader.Width = 87;
            // 
            // _messageColumnHeader
            // 
            this._messageColumnHeader.Text = "Message";
            this._messageColumnHeader.Width = 99;
            // 
            // _descriptionColumnHeader
            // 
            this._descriptionColumnHeader.Text = "Description";
            this._descriptionColumnHeader.Width = 293;
            // 
            // _listViewContextMenuStrip
            // 
            this._listViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSelectedCameraToolStripMenuItem});
            this._listViewContextMenuStrip.Name = "_listViewContextMenuStrip";
            this._listViewContextMenuStrip.Size = new System.Drawing.Size(195, 26);
            // 
            // openSelectedCameraToolStripMenuItem
            // 
            this.openSelectedCameraToolStripMenuItem.Name = "openSelectedCameraToolStripMenuItem";
            this.openSelectedCameraToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openSelectedCameraToolStripMenuItem.Text = "Open Selected Camera";
            this.openSelectedCameraToolStripMenuItem.Click += new System.EventHandler(this.OnOpenSelectedCameraClick);
            // 
            // ReportToolView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._messagesListView);
            this.Controls.Add(this._toolMainStrip);
            this.Name = "ReportToolView";
            this.Size = new System.Drawing.Size(677, 219);
            this._toolMainStrip.ResumeLayout(false);
            this._toolMainStrip.PerformLayout();
            this._listViewContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolMainStrip;
        private System.Windows.Forms.ToolStripLabel _serverToolStripLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel _connectionStatusToolStripLabel;
        private System.Windows.Forms.ToolStripButton _connectToolStripButton;
        private System.Windows.Forms.ToolStripButton _disconnectToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel _statusToolStripLabel;
        private ListViewNF _messagesListView;
        private System.Windows.Forms.ColumnHeader _cameraIdColumnHeader;
        private System.Windows.Forms.ColumnHeader _importanceColumnHeader;
        private System.Windows.Forms.ColumnHeader _messageColumnHeader;
        private System.Windows.Forms.ColumnHeader _descriptionColumnHeader;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel _viewToolStripLabel;
        private System.Windows.Forms.ToolStripButton _openCameraToolStripButton;
        private System.Windows.Forms.ContextMenuStrip _listViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openSelectedCameraToolStripMenuItem;
    }
}
