using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using HDE.Platform.Logging;
using Rhino.Queues.Model;
using Rhino.Queues.Protocol;

namespace MessageRouter.Common
{
    public class MessageRouterResults : IMessageRouterResults
    {
        #region Fields

        private Sender _sender;

        #endregion

        #region Properties

        protected ILog Log { get; private set; }
        public RhinoQueueConfiguration DestinationConfiguration { get; private set; }

        #endregion

        #region Constructors

        public MessageRouterResults(ILog log, RhinoQueueConfiguration destinationConfiguration)
        {
            Log = log;
            DestinationConfiguration = destinationConfiguration;
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (_sender != null)
            {
                _sender.Dispose();
            }
        }

        public void SendText(string text)
        {
            SendBinary(Encoding.Unicode.GetBytes(text));
        }

        public virtual void SendBinary(byte[] data)
        {
            if (_sender == null) // We shouldn't create data transferring immidietly
            {
                _sender = new Sender
                    {
                        Destination = new Endpoint(DestinationConfiguration.Host, DestinationConfiguration.Port),
                        Failure = HandleException,
                    };
            }

            _sender.Messages = new[] { new Message
                {
                    Id = MessageId.GenerateRandom(),
                    Queue = DestinationConfiguration.QueueName,
                    Data = data,
                    SentAt = DateTime.UtcNow
                }};

            _sender.Send();
        }

        public void SendBinary(object data)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, data);
                SendBinary(stream.GetBuffer());
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void HandleException(Exception unhandledException)
        {
            Log.Error(unhandledException);
        }

        #endregion
    }
}
