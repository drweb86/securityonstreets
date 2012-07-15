namespace HDE.IpCamClientServer.Client.View
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
            this._tabControl = new System.Windows.Forms.TabControl();
            this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Location = new System.Drawing.Point(0, 24);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(622, 251);
            this._tabControl.TabIndex = 0;
            // 
            // _mainMenuStrip
            // 
            this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this._mainMenuStrip.Name = "_mainMenuStrip";
            this._mainMenuStrip.Size = new System.Drawing.Size(622, 24);
            this._mainMenuStrip.TabIndex = 1;
            this._mainMenuStrip.Text = "mainMenu";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 275);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "Camera Client App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.MenuStrip _mainMenuStrip;
    }
}

