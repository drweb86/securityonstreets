namespace HDE.IpCamClientServer.Server.ServerC.View
{
    partial class DebugViewHostForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugViewHostForm));
            this._mainToolStrip = new System.Windows.Forms.ToolStrip();
            this._takeScreenshotsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._preparingLabel = new System.Windows.Forms.Label();
            this._mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainToolStrip
            // 
            this._mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._takeScreenshotsToolStripButton});
            this._mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this._mainToolStrip.Name = "_mainToolStrip";
            this._mainToolStrip.Size = new System.Drawing.Size(505, 25);
            this._mainToolStrip.TabIndex = 1;
            this._mainToolStrip.Text = "toolStrip1";
            // 
            // _takeScreenshotsToolStripButton
            // 
            this._takeScreenshotsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._takeScreenshotsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("_takeScreenshotsToolStripButton.Image")));
            this._takeScreenshotsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._takeScreenshotsToolStripButton.Name = "_takeScreenshotsToolStripButton";
            this._takeScreenshotsToolStripButton.Size = new System.Drawing.Size(100, 22);
            this._takeScreenshotsToolStripButton.Text = "Take Screenshot!";
            this._takeScreenshotsToolStripButton.Click += new System.EventHandler(this.OnTakeScreenshotsClick);
            // 
            // _preparingLabel
            // 
            this._preparingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._preparingLabel.BackColor = System.Drawing.Color.Transparent;
            this._preparingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._preparingLabel.ForeColor = System.Drawing.Color.OrangeRed;
            this._preparingLabel.Location = new System.Drawing.Point(0, 25);
            this._preparingLabel.Name = "_preparingLabel";
            this._preparingLabel.Size = new System.Drawing.Size(505, 316);
            this._preparingLabel.TabIndex = 3;
            this._preparingLabel.Text = "Preparing Data";
            this._preparingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DebugViewHostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 350);
            this.Controls.Add(this._preparingLabel);
            this.Controls.Add(this._mainToolStrip);
            this.IsMdiContainer = true;
            this.Name = "DebugViewHostForm";
            this.Text = "Data is being collected, please wait...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this._mainToolStrip.ResumeLayout(false);
            this._mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _mainToolStrip;
        private System.Windows.Forms.ToolStripButton _takeScreenshotsToolStripButton;
        private System.Windows.Forms.Label _preparingLabel;
    }
}