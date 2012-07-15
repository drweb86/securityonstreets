namespace LocalSetup
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._settingsGroupBox = new System.Windows.Forms.GroupBox();
            this._ipCamGroupBox = new System.Windows.Forms.GroupBox();
            this._ipCameraEmulatorPortLabel = new System.Windows.Forms.Label();
            this._ipCamNameTextBox = new System.Windows.Forms.TextBox();
            this._ipCamNameLabel = new System.Windows.Forms.Label();
            this._ipCameraEmulatorPortTextBox = new System.Windows.Forms.TextBox();
            this._ipCamIdLabel = new System.Windows.Forms.Label();
            this._ipCamIdTextBox = new System.Windows.Forms.TextBox();
            this._clientInputPortTextBox = new System.Windows.Forms.TextBox();
            this._clientResultSubscriptionPortLabel = new System.Windows.Forms.Label();
            this._messageRouterInputPortTextBox = new System.Windows.Forms.TextBox();
            this._messageRouterIncomePortLabel = new System.Windows.Forms.Label();
            this._userPasswordTextBox = new System.Windows.Forms.TextBox();
            this._userPasswordLabel = new System.Windows.Forms.Label();
            this._prepareButton = new System.Windows.Forms.Button();
            this._settingsGroupBox.SuspendLayout();
            this._ipCamGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _settingsGroupBox
            // 
            this._settingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._settingsGroupBox.Controls.Add(this._ipCamGroupBox);
            this._settingsGroupBox.Controls.Add(this._clientInputPortTextBox);
            this._settingsGroupBox.Controls.Add(this._clientResultSubscriptionPortLabel);
            this._settingsGroupBox.Controls.Add(this._messageRouterInputPortTextBox);
            this._settingsGroupBox.Controls.Add(this._messageRouterIncomePortLabel);
            this._settingsGroupBox.Controls.Add(this._userPasswordTextBox);
            this._settingsGroupBox.Controls.Add(this._userPasswordLabel);
            this._settingsGroupBox.Location = new System.Drawing.Point(12, 12);
            this._settingsGroupBox.Name = "_settingsGroupBox";
            this._settingsGroupBox.Size = new System.Drawing.Size(542, 220);
            this._settingsGroupBox.TabIndex = 0;
            this._settingsGroupBox.TabStop = false;
            this._settingsGroupBox.Text = "Settings";
            // 
            // _ipCamGroupBox
            // 
            this._ipCamGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ipCamGroupBox.Controls.Add(this._ipCameraEmulatorPortLabel);
            this._ipCamGroupBox.Controls.Add(this._ipCamNameTextBox);
            this._ipCamGroupBox.Controls.Add(this._ipCamNameLabel);
            this._ipCamGroupBox.Controls.Add(this._ipCameraEmulatorPortTextBox);
            this._ipCamGroupBox.Controls.Add(this._ipCamIdLabel);
            this._ipCamGroupBox.Controls.Add(this._ipCamIdTextBox);
            this._ipCamGroupBox.Location = new System.Drawing.Point(6, 103);
            this._ipCamGroupBox.Name = "_ipCamGroupBox";
            this._ipCamGroupBox.Size = new System.Drawing.Size(525, 106);
            this._ipCamGroupBox.TabIndex = 4;
            this._ipCamGroupBox.TabStop = false;
            this._ipCamGroupBox.Text = "IP-Camera (Emulator)";
            // 
            // _ipCameraEmulatorPortLabel
            // 
            this._ipCameraEmulatorPortLabel.AutoSize = true;
            this._ipCameraEmulatorPortLabel.Location = new System.Drawing.Point(6, 23);
            this._ipCameraEmulatorPortLabel.Name = "_ipCameraEmulatorPortLabel";
            this._ipCameraEmulatorPortLabel.Size = new System.Drawing.Size(29, 13);
            this._ipCameraEmulatorPortLabel.TabIndex = 6;
            this._ipCameraEmulatorPortLabel.Text = "Port:";
            // 
            // _ipCamNameTextBox
            // 
            this._ipCamNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ipCamNameTextBox.Location = new System.Drawing.Point(263, 72);
            this._ipCamNameTextBox.Name = "_ipCamNameTextBox";
            this._ipCamNameTextBox.Size = new System.Drawing.Size(256, 20);
            this._ipCamNameTextBox.TabIndex = 7;
            this._ipCamNameTextBox.Text = "Demo Camera 3";
            // 
            // _ipCamNameLabel
            // 
            this._ipCamNameLabel.AutoSize = true;
            this._ipCamNameLabel.Location = new System.Drawing.Point(7, 75);
            this._ipCamNameLabel.Name = "_ipCamNameLabel";
            this._ipCamNameLabel.Size = new System.Drawing.Size(38, 13);
            this._ipCamNameLabel.TabIndex = 14;
            this._ipCamNameLabel.Text = "Name:";
            // 
            // _ipCameraEmulatorPortTextBox
            // 
            this._ipCameraEmulatorPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ipCameraEmulatorPortTextBox.Location = new System.Drawing.Point(263, 20);
            this._ipCameraEmulatorPortTextBox.Name = "_ipCameraEmulatorPortTextBox";
            this._ipCameraEmulatorPortTextBox.Size = new System.Drawing.Size(256, 20);
            this._ipCameraEmulatorPortTextBox.TabIndex = 5;
            this._ipCameraEmulatorPortTextBox.Text = "2000";
            // 
            // _ipCamIdLabel
            // 
            this._ipCamIdLabel.AutoSize = true;
            this._ipCamIdLabel.Location = new System.Drawing.Point(7, 49);
            this._ipCamIdLabel.Name = "_ipCamIdLabel";
            this._ipCamIdLabel.Size = new System.Drawing.Size(80, 13);
            this._ipCamIdLabel.TabIndex = 12;
            this._ipCamIdLabel.Text = "String Identifier:";
            // 
            // _ipCamIdTextBox
            // 
            this._ipCamIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._ipCamIdTextBox.Location = new System.Drawing.Point(263, 46);
            this._ipCamIdTextBox.Name = "_ipCamIdTextBox";
            this._ipCamIdTextBox.Size = new System.Drawing.Size(256, 20);
            this._ipCamIdTextBox.TabIndex = 6;
            this._ipCamIdTextBox.Text = "DemoCam-3";
            // 
            // _clientInputPortTextBox
            // 
            this._clientInputPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._clientInputPortTextBox.Location = new System.Drawing.Point(269, 77);
            this._clientInputPortTextBox.Name = "_clientInputPortTextBox";
            this._clientInputPortTextBox.Size = new System.Drawing.Size(268, 20);
            this._clientInputPortTextBox.TabIndex = 3;
            this._clientInputPortTextBox.Text = "2030";
            // 
            // _clientResultSubscriptionPortLabel
            // 
            this._clientResultSubscriptionPortLabel.AutoSize = true;
            this._clientResultSubscriptionPortLabel.Location = new System.Drawing.Point(8, 80);
            this._clientResultSubscriptionPortLabel.Name = "_clientResultSubscriptionPortLabel";
            this._clientResultSubscriptionPortLabel.Size = new System.Drawing.Size(86, 13);
            this._clientResultSubscriptionPortLabel.TabIndex = 10;
            this._clientResultSubscriptionPortLabel.Text = "Client: input port:";
            // 
            // _messageRouterInputPortTextBox
            // 
            this._messageRouterInputPortTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._messageRouterInputPortTextBox.Location = new System.Drawing.Point(269, 51);
            this._messageRouterInputPortTextBox.Name = "_messageRouterInputPortTextBox";
            this._messageRouterInputPortTextBox.Size = new System.Drawing.Size(267, 20);
            this._messageRouterInputPortTextBox.TabIndex = 2;
            this._messageRouterInputPortTextBox.Text = "2020";
            // 
            // _messageRouterIncomePortLabel
            // 
            this._messageRouterIncomePortLabel.AutoSize = true;
            this._messageRouterIncomePortLabel.Location = new System.Drawing.Point(7, 54);
            this._messageRouterIncomePortLabel.Name = "_messageRouterIncomePortLabel";
            this._messageRouterIncomePortLabel.Size = new System.Drawing.Size(138, 13);
            this._messageRouterIncomePortLabel.TabIndex = 8;
            this._messageRouterIncomePortLabel.Text = "Message Queue: input port:";
            // 
            // _userPasswordTextBox
            // 
            this._userPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._userPasswordTextBox.Location = new System.Drawing.Point(269, 25);
            this._userPasswordTextBox.Name = "_userPasswordTextBox";
            this._userPasswordTextBox.PasswordChar = '*';
            this._userPasswordTextBox.Size = new System.Drawing.Size(266, 20);
            this._userPasswordTextBox.TabIndex = 1;
            // 
            // _userPasswordLabel
            // 
            this._userPasswordLabel.AutoSize = true;
            this._userPasswordLabel.Location = new System.Drawing.Point(6, 28);
            this._userPasswordLabel.Name = "_userPasswordLabel";
            this._userPasswordLabel.Size = new System.Drawing.Size(169, 13);
            this._userPasswordLabel.TabIndex = 4;
            this._userPasswordLabel.Text = "Your computer account password:";
            // 
            // _prepareButton
            // 
            this._prepareButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._prepareButton.Location = new System.Drawing.Point(473, 247);
            this._prepareButton.Name = "_prepareButton";
            this._prepareButton.Size = new System.Drawing.Size(75, 23);
            this._prepareButton.TabIndex = 8;
            this._prepareButton.Text = "Create";
            this._prepareButton.UseVisualStyleBackColor = true;
            this._prepareButton.Click += new System.EventHandler(this.OnPrepareButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 282);
            this.Controls.Add(this._prepareButton);
            this.Controls.Add(this._settingsGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(582, 320);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Local Setup - Create Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this._settingsGroupBox.ResumeLayout(false);
            this._settingsGroupBox.PerformLayout();
            this._ipCamGroupBox.ResumeLayout(false);
            this._ipCamGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _settingsGroupBox;
        private System.Windows.Forms.TextBox _clientInputPortTextBox;
        private System.Windows.Forms.Label _clientResultSubscriptionPortLabel;
        private System.Windows.Forms.TextBox _messageRouterInputPortTextBox;
        private System.Windows.Forms.Label _messageRouterIncomePortLabel;
        private System.Windows.Forms.TextBox _ipCameraEmulatorPortTextBox;
        private System.Windows.Forms.Label _ipCameraEmulatorPortLabel;
        private System.Windows.Forms.TextBox _userPasswordTextBox;
        private System.Windows.Forms.Label _userPasswordLabel;
        private System.Windows.Forms.TextBox _ipCamNameTextBox;
        private System.Windows.Forms.Label _ipCamNameLabel;
        private System.Windows.Forms.TextBox _ipCamIdTextBox;
        private System.Windows.Forms.Label _ipCamIdLabel;
        private System.Windows.Forms.Button _prepareButton;
        private System.Windows.Forms.GroupBox _ipCamGroupBox;
    }
}

