using System;
using System.Collections.Generic;
using System.Threading;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using HDE.IpCamClientServer.Server.ServerC.View;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class DebugInterceptor: IInterceptor, IDisposable
    {
        #region Properties

        private volatile Dictionary<string, IDebugView> _debugViews;
        private readonly Dictionary<string, ImageSource> _imageSources;
        private readonly List<Thread> _threads;

        #endregion

        #region Constructors

        public DebugInterceptor()
        {
            _debugViews = new Dictionary<string, IDebugView>();
            _imageSources = new Dictionary<string, ImageSource>();
            _threads = new List<Thread>();
        }

        #endregion

        #region Public Methods

        public void Initialize(string[] keys)
        {
            int windowNo = -1;
            foreach (var key in keys)
            {
                windowNo++;
                var thread = new Thread(OnPerformThreadJob);
                thread.SetApartmentState(ApartmentState.STA);
                _threads.Add(thread);
                _imageSources.Add(key, new ImageSource());
                thread.Start(new ThreadTask(windowNo, key, _imageSources[key]));
            }
        }

        #endregion

        #region IInterceptor Implementation

        public void Intercept(string key, byte[] image)
        {
            if (_debugViews.ContainsKey(key))
            {
                _imageSources[key].NewFrameReceived(this, new NewFrameEventArgs(image));
            }
/* Yes, there're no memory leaks (but .Net usually blocks)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
*/
        }

        #endregion

        #region Private Methods

        private void OnPerformThreadJob(object task)
        {
            var taskTyped = (ThreadTask) task;
            IDebugView operationWindow = new DebugViewForm();
            _debugViews.Add(taskTyped.Key, operationWindow);
            operationWindow.Initialize(taskTyped.WindowNo, taskTyped.Key, taskTyped.ImageSource);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            foreach (var thread in _threads)
            {
                thread.Abort();
            }

            foreach (var item in _debugViews)
            {
                item.Value.Dispose();
            }
        }

        #endregion

        #region Nested Types

        class ThreadTask
        {
            #region Constructors

            public ThreadTask(int windowNo, string key, ImageSource imageSource)
            {
                WindowNo = windowNo;
                Key = key;
                ImageSource = imageSource;
            }

            #endregion

            #region Properties

            public int WindowNo { get; private set; }
            public string Key { get; private set; }
            public ImageSource ImageSource { get; private set; }

            #endregion
        }

        #endregion
    }
}