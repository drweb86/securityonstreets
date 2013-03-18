using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.ServerC.Controller;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    partial class DebugViewHostForm : Form
    {
        #region Fields

        private int _windowNo = -1;
        private readonly Dictionary<string, DebugViewForm> _debugViews = new Dictionary<string, DebugViewForm>();
        private ImageSource _imageSource;

        #endregion

        #region Constructors

        public DebugViewHostForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Initialize(ImageSource imageSource)
        {
            BeforeDispose();

            _imageSource = imageSource;
            _imageSource.NewFrameReceived += OnNewFrameReceived;
            _imageSource.DisposeRequested += OnCloseRequested;
        }

        #endregion

        #region Private Methods

        private void OnCloseRequested(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // that ninjutsu helps with OutOfMemory!
                BeginInvoke(new EventHandler<EventArgs>(OnCloseRequested), sender, e)
                    .AsyncWaitHandle.WaitOne();
            }
            else
            {
                DialogResult = DialogResult.OK;
                BeforeDispose();
                Close();
            }
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
                if (!_debugViews.ContainsKey(e.Key))
                {
                    if (_debugViews.Count == 0)
                    {
                        Text = "Debugging";
                    }
                    _windowNo++;
                    var debugWindow = new DebugViewForm(_windowNo);
                    debugWindow.MdiParent = this;
                    debugWindow.Text = e.Key;
                    _debugViews.Add(e.Key, debugWindow);
                    debugWindow.Show();
                }
                _debugViews[e.Key].Update(e.Frame);
            }
        }

        private void BeforeDispose()
        {
            if (_imageSource != null)
            {
                _imageSource.DisposeRequested -= OnCloseRequested;
                _imageSource.NewFrameReceived -= OnNewFrameReceived;
                _imageSource = null;
            }
            _windowNo = -1;
        
        }

        private void OnTakeScreenshotsClick(object sender, EventArgs e)
        {
            this.TakeMdiScreenshots();
        }

        #endregion
    }
}
