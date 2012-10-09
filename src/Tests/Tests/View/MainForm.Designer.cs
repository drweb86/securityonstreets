namespace WindowsFormsApplication1
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
            this._bitmapToGrayscale = new System.Windows.Forms.Button();
            this.calculateMeanAverageIntensitiesButton_ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _bitmapToGrayscale
            // 
            this._bitmapToGrayscale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._bitmapToGrayscale.Location = new System.Drawing.Point(383, 12);
            this._bitmapToGrayscale.Name = "_bitmapToGrayscale";
            this._bitmapToGrayscale.Size = new System.Drawing.Size(199, 23);
            this._bitmapToGrayscale.TabIndex = 0;
            this._bitmapToGrayscale.Text = "Bitmap to GrayScale";
            this._bitmapToGrayscale.UseVisualStyleBackColor = true;
            this._bitmapToGrayscale.Click += new System.EventHandler(this.OnImageToGrayscaleClick);
            // 
            // calculateMeanAverageIntensitiesButton_
            // 
            this.calculateMeanAverageIntensitiesButton_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calculateMeanAverageIntensitiesButton_.Location = new System.Drawing.Point(383, 41);
            this.calculateMeanAverageIntensitiesButton_.Name = "calculateMeanAverageIntensitiesButton_";
            this.calculateMeanAverageIntensitiesButton_.Size = new System.Drawing.Size(199, 23);
            this.calculateMeanAverageIntensitiesButton_.TabIndex = 1;
            this.calculateMeanAverageIntensitiesButton_.Text = "Mean and variance M9";
            this.calculateMeanAverageIntensitiesButton_.UseVisualStyleBackColor = true;
            this.calculateMeanAverageIntensitiesButton_.Click += new System.EventHandler(this.OnCalculateMeanAndVarianceClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 420);
            this.Controls.Add(this.calculateMeanAverageIntensitiesButton_);
            this.Controls.Add(this._bitmapToGrayscale);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tests";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _bitmapToGrayscale;
        private System.Windows.Forms.Button calculateMeanAverageIntensitiesButton_;
    }
}

