namespace HDE.IpCamClientServer.Client.CamViewer.View
{
    partial class CamViewToolView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._camerasListBox = new System.Windows.Forms.ListBox();
            this._selectedCameraInfoLabel = new System.Windows.Forms.Label();
            this._camerasToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this._addToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._deleteSelectedCameraToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._editSelectedCameraSettingsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._viewPictureBox = new System.Windows.Forms.PictureBox();
            this._cameraViewToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this._connectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._disconnectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._connectStatusToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this._camerasToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._viewPictureBox)).BeginInit();
            this._cameraViewToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this._camerasToolStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this._viewPictureBox);
            this.splitContainer1.Panel2.Controls.Add(this._cameraViewToolStrip);
            this.splitContainer1.Size = new System.Drawing.Size(639, 273);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._camerasListBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel2.Controls.Add(this._selectedCameraInfoLabel);
            this.splitContainer2.Size = new System.Drawing.Size(215, 248);
            this.splitContainer2.SplitterDistance = 146;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 1;
            // 
            // _camerasListBox
            // 
            this._camerasListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._camerasListBox.FormattingEnabled = true;
            this._camerasListBox.Location = new System.Drawing.Point(0, 0);
            this._camerasListBox.Name = "_camerasListBox";
            this._camerasListBox.Size = new System.Drawing.Size(215, 146);
            this._camerasListBox.TabIndex = 0;
            this._camerasListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedCameraChanged);
            // 
            // _selectedCameraInfoLabel
            // 
            this._selectedCameraInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._selectedCameraInfoLabel.Location = new System.Drawing.Point(14, 10);
            this._selectedCameraInfoLabel.Name = "_selectedCameraInfoLabel";
            this._selectedCameraInfoLabel.Size = new System.Drawing.Size(189, 78);
            this._selectedCameraInfoLabel.TabIndex = 0;
            this._selectedCameraInfoLabel.Text = "<Selected Camera Info>";
            // 
            // _camerasToolStrip
            // 
            this._camerasToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this._addToolStripButton,
            this._deleteSelectedCameraToolStripButton,
            this._editSelectedCameraSettingsToolStripButton});
            this._camerasToolStrip.Location = new System.Drawing.Point(0, 0);
            this._camerasToolStrip.Name = "_camerasToolStrip";
            this._camerasToolStrip.Size = new System.Drawing.Size(215, 25);
            this._camerasToolStrip.TabIndex = 0;
            this._camerasToolStrip.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(62, 22);
            this.toolStripLabel1.Text = "CAMERAS";
            // 
            // _addToolStripButton
            // 
            this._addToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._addToolStripButton.Image = global::HDE.IpCamClientServer.Client.CamViewer.Icons.Add;
            this._addToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._addToolStripButton.Name = "_addToolStripButton";
            this._addToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._addToolStripButton.Text = "Add New Camera";
            this._addToolStripButton.Click += new System.EventHandler(this.OnAddCamera);
            // 
            // _deleteSelectedCameraToolStripButton
            // 
            this._deleteSelectedCameraToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._deleteSelectedCameraToolStripButton.Image = global::HDE.IpCamClientServer.Client.CamViewer.Icons.Remove;
            this._deleteSelectedCameraToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deleteSelectedCameraToolStripButton.Name = "_deleteSelectedCameraToolStripButton";
            this._deleteSelectedCameraToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._deleteSelectedCameraToolStripButton.Text = "Delete Selected Camera";
            this._deleteSelectedCameraToolStripButton.Click += new System.EventHandler(this.OnDeleteSelectedCamera);
            // 
            // _editSelectedCameraSettingsToolStripButton
            // 
            this._editSelectedCameraSettingsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._editSelectedCameraSettingsToolStripButton.Image = global::HDE.IpCamClientServer.Client.CamViewer.Icons.Modify;
            this._editSelectedCameraSettingsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._editSelectedCameraSettingsToolStripButton.Name = "_editSelectedCameraSettingsToolStripButton";
            this._editSelectedCameraSettingsToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._editSelectedCameraSettingsToolStripButton.Text = "Edit Selected Camera Settings";
            this._editSelectedCameraSettingsToolStripButton.Click += new System.EventHandler(this.OnEditSelectedCameraSettings);
            // 
            // _viewPictureBox
            // 
            this._viewPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewPictureBox.Location = new System.Drawing.Point(0, 25);
            this._viewPictureBox.Name = "_viewPictureBox";
            this._viewPictureBox.Size = new System.Drawing.Size(420, 248);
            this._viewPictureBox.TabIndex = 1;
            this._viewPictureBox.TabStop = false;
            // 
            // _cameraViewToolStrip
            // 
            this._cameraViewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this._connectToolStripButton,
            this._disconnectToolStripButton,
            this._connectStatusToolStripLabel});
            this._cameraViewToolStrip.Location = new System.Drawing.Point(0, 0);
            this._cameraViewToolStrip.Name = "_cameraViewToolStrip";
            this._cameraViewToolStrip.Size = new System.Drawing.Size(420, 25);
            this._cameraViewToolStrip.TabIndex = 0;
            this._cameraViewToolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(88, 22);
            this.toolStripLabel2.Text = "CAMERA VIEW";
            // 
            // _connectToolStripButton
            // 
            this._connectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._connectToolStripButton.Image = global::HDE.IpCamClientServer.Client.CamViewer.Icons.Start;
            this._connectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._connectToolStripButton.Name = "_connectToolStripButton";
            this._connectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._connectToolStripButton.Text = "Connect";
            this._connectToolStripButton.Click += new System.EventHandler(this.OnConnectToSelectedCam);
            // 
            // _disconnectToolStripButton
            // 
            this._disconnectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._disconnectToolStripButton.Image = global::HDE.IpCamClientServer.Client.CamViewer.Icons.Stop;
            this._disconnectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._disconnectToolStripButton.Name = "_disconnectToolStripButton";
            this._disconnectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this._disconnectToolStripButton.Text = "Disconnect";
            this._disconnectToolStripButton.Click += new System.EventHandler(this.OnDisconnectClick);
            // 
            // _connectStatusToolStripLabel
            // 
            this._connectStatusToolStripLabel.Name = "_connectStatusToolStripLabel";
            this._connectStatusToolStripLabel.Size = new System.Drawing.Size(55, 22);
            this._connectStatusToolStripLabel.Text = "<Status>";
            // 
            // CamViewToolView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CamViewToolView";
            this.Size = new System.Drawing.Size(639, 273);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this._camerasToolStrip.ResumeLayout(false);
            this._camerasToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._viewPictureBox)).EndInit();
            this._cameraViewToolStrip.ResumeLayout(false);
            this._cameraViewToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip _camerasToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton _addToolStripButton;
        private System.Windows.Forms.ToolStripButton _deleteSelectedCameraToolStripButton;
        private System.Windows.Forms.ToolStrip _cameraViewToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton _connectToolStripButton;
        private System.Windows.Forms.ToolStripButton _disconnectToolStripButton;
        private System.Windows.Forms.ToolStripLabel _connectStatusToolStripLabel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox _camerasListBox;
        private System.Windows.Forms.Label _selectedCameraInfoLabel;
        private System.Windows.Forms.PictureBox _viewPictureBox;
        private System.Windows.Forms.ToolStripButton _editSelectedCameraSettingsToolStripButton;

    }
}
