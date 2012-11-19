using System;
using System.Drawing;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using HDE.IpCamClientServer.Server.ServerC.Controller;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    partial class DebugViewForm : Form, IDebugView
    {
        #region Fields

        private ImageSource _source;
        private int _windowNo;

        #endregion

        #region Constructors

        public DebugViewForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        void IDebugView.Initialize(int windowNo, string title, ImageSource source)
        {
            _windowNo = windowNo;
            Text = title;
            _source = source;
            _source.NewFrameReceived += OnNewFrameReceived;
            ShowDialog();
        }

        private void OnNewFrameReceived(object sender, NewFrameEventArgs e)
        {
            if (InvokeRequired)
            {
                // that ninjutsu helps with OutOfMemory!
                BeginInvoke(new EventHandler<NewFrameEventArgs>(OnNewFrameReceived), sender, e)
                    .AsyncWaitHandle.WaitOne();
            }
            else
            {
                Update(e.Frame);
            }
        }

        private readonly object _sync = new object();
        private void Update(byte[] image)
        {
            lock (_sync)
            {
                if (BackgroundImage != null)
                {
                    Image imageOld = BackgroundImage;
                    BackgroundImage = null;
                    imageOld.Dispose();
                }
                Image imageImg = ImageHelper.FromBytes(image);

                if (Width < (imageImg.Width + 30) ||
                    Height < (imageImg.Height + 30))
                {
                    Width = imageImg.Width + 30;
                    Left = (_windowNo%2)*Width;

                    Height = imageImg.Height + 30;
                    Top = (_windowNo / 2) * Height;
                }

                BackgroundImage = imageImg;
            }
            Application.DoEvents();
        }
        
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_source != null)
            {
                _source.NewFrameReceived -= OnNewFrameReceived;
                _source = null;
            }
        }

        #endregion

        private void OnTimerTick(object sender, EventArgs e)
        {
            Application.DoEvents();
        }
    }
}
