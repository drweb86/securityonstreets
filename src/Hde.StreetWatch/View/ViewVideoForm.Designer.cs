namespace Hde.StreetWatch.View
{
    partial class ViewVideoForm
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
            this._viewPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._viewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _viewPictureBox
            // 
            this._viewPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewPictureBox.Location = new System.Drawing.Point(0, 0);
            this._viewPictureBox.Name = "_viewPictureBox";
            this._viewPictureBox.Size = new System.Drawing.Size(403, 307);
            this._viewPictureBox.TabIndex = 0;
            this._viewPictureBox.TabStop = false;
            // 
            // ViewVideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 307);
            this.Controls.Add(this._viewPictureBox);
            this.Name = "ViewVideoForm";
            this.Text = "View MPEG Video";
            ((System.ComponentModel.ISupportInitialize)(this._viewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _viewPictureBox;
    }
}