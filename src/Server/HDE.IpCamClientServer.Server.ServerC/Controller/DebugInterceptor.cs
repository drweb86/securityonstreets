using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using HDE.IpCamClientServer.Server.ServerC.View;

namespace HDE.IpCamClientServer.Server.ServerC.Controller
{
    class DebugInterceptor: IInterceptor, IDisposable
    {
        #region Properties

        private readonly Dictionary<string, IDebugView> _debugViews;
        private readonly List<Thread> _threads;

        #endregion

        #region Constructors

        public DebugInterceptor()
        {
            _debugViews = new Dictionary<string, IDebugView>();
            _threads = new List<Thread>();

            var thread = new Thread(OnPerformThreadJob);
            thread.SetApartmentState(ApartmentState.STA);
            _threads.Add(thread);
            thread.Start(new ThreadTask(MovementHandler.DifferenceImage, "Difference Image"));
        }

        #endregion

        #region IInterceptor Implementation

        public void Intercept(string key, Image image)
        {
            if (_debugViews.ContainsKey(key))
            {
                var view = _debugViews[key];
                view.Update(image);
            }
        }

        #endregion

        #region Private Methods

        private void OnPerformThreadJob(object task)
        {
            var taskTyped = (ThreadTask) task;
            IDebugView operationWindow = new DebugViewForm();
            _debugViews.Add(taskTyped.Key, operationWindow);
            operationWindow.Initialize(taskTyped.Title);
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

            public ThreadTask(string key, string title)
            {
                Key = key;
                Title = title;
            }

            #endregion

            #region Properties

            public string Key { get; private set; }
            public string Title { get; private set; }

            #endregion
        }

        #endregion
    }
}