using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using HDE.IpCamClientServer.Server.ServerC.View;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class DebugInterceptor: IInterceptor, IDisposable
    {
        #region Properties

        private readonly ImageSource _imageSource = new ImageSource();
        private Thread _thread;
        private readonly List<string> _supportedKeys = new List<string>();

        #endregion

        #region Public Methods

        public void Initialize(string[] keys)
        {
            _supportedKeys.AddRange(keys);
            _thread = new Thread(OnPerformThreadJob);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start(_imageSource);
        }

        #endregion

        #region IInterceptor Implementation

        public void Intercept(string status)
        {
            try
            {
                _imageSource.NewStatusReceived(this, new NewStatusEventArgs(status));
            }
            catch (NullReferenceException) // user closed window
            {
            }
        }

        public void Intercept(int currentProcessed, int totalToProcess)
        {
            try
            {
                _imageSource.NewProgressReceived(this, new NewProgressEventArgs(currentProcessed, totalToProcess));
            }
            catch (NullReferenceException) // user closed window
            {
            }
        }

        public void Intercept(string key, byte[] image)
        {
            try
            {
                if (_supportedKeys.Contains(key))
                {
                    _imageSource.NewFrameReceived(this, new NewFrameEventArgs(key, image));
                }
            }
            catch (NullReferenceException) // user closed window
            {
            }
        }

        #endregion

        #region Private Methods

        private void OnPerformThreadJob(object task)
        {
            var taskTyped = (ImageSource) task;
            var form = new DebugViewHostForm();
            form.Initialize(taskTyped);
            Application.Run(form);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            _imageSource.DisposeRequested(this, new EventArgs());
            _thread.Join();
        }

        #endregion
    }
}