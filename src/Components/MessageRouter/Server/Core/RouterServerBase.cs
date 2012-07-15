using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using HDE.Platform.Logging;
using MessageRouter.Common;
using Rhino.Queues;
using Rhino.Queues.Model;

namespace MessageRouter.Server.Core
{
    public abstract class RouterServerBase : IRouterServer
    {
        #region Fields

        private readonly ServerSettings _settings;
        private readonly QueueManager _queueManager;
        private readonly List<IMessageRouterResults> _subscribers;

        #endregion

        #region Properties

        protected ILog Log { get; private set; }

        #endregion

        #region Constructors

        protected RouterServerBase(ILog log, ServerSettings settings)
        {
            Log = log;
            _settings = settings;
            _queueManager = RhinoHelper.OpenServer(_settings.Server);

            _subscribers = new List<IMessageRouterResults>();
            foreach (var listenerSettings in settings.Listeners)
            {
                _subscribers.Add(new MessageRouterResults(log, listenerSettings));
            }
        }

        #endregion

        public void Dispose()
        {
            _queueManager.Dispose();
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var message = _queueManager.Receive(_settings.Server.QueueName);

                        ProcessMessage(message);

                        foreach (var subscriber in _subscribers)
                        {
                            try
                            {
                                subscriber.SendBinary(message.Data);
                            }
                            catch (ThreadAbortException)
                            {
                                throw;
                            }
                            catch (Exception e)
                            {
                                HandleException(subscriber.DestinationConfiguration, e);
                            }
                        }

                        transactionScope.Complete();
                    }
                }
                catch (TransactionAbortedException)
                {
                }
            }
        }

        protected abstract void ProcessMessage(Message message);
        
        protected virtual void HandleException(RhinoQueueConfiguration configuration, Exception unhandledException)
        {
            Log.Error("[ERROR] {0}: {1}", configuration.Name, unhandledException);
        }
    }
}
