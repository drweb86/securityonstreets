using System;
using System.Transactions;
using HDE.Platform.Logging;
using Rhino.Queues;
using Rhino.Queues.Model;

namespace MessageRouter.Common
{
    public abstract class MessageRouterSubscriberBase : IMessageRouterSubscriber
    {
        #region Fields

        private readonly RhinoQueueConfiguration _subscriptionQueue;
        private QueueManager queueManager_;

        #endregion

        #region Properties

        protected ILog Log { get; private set; }
        public bool RequestStopping { get; set; }

        #endregion

        #region Constructors

        protected MessageRouterSubscriberBase(ILog log, RhinoQueueConfiguration subscriptionQueue)
        {
            Log = log;
            _subscriptionQueue = subscriptionQueue;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (queueManager_ == null)
            {
                queueManager_ = RhinoHelper.OpenServer(_subscriptionQueue);
            }

            while (!RequestStopping)
            {
                try
                {
                    using (var tx = new TransactionScope())
                    {
                        try
                        {
                            var message = queueManager_.Receive(_subscriptionQueue.QueueName);
                            ProcessMessage(message);
                        }
                        catch (TimeoutException) // that's for make it stoppable
                        {
                        }
                        tx.Complete();
                    }
                }
                catch (TransactionAbortedException)
                {
                }
            }
        }

        #endregion

        #region Protected Methods

        protected abstract void ProcessMessage(Message message);

        #endregion

        public void Dispose()
        {
            if (queueManager_ != null)
            {
                queueManager_.Dispose();
                queueManager_ = null;
            }
        }
    }
}