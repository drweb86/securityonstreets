using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MessageRouter.Common;

namespace MessageRouter.Server.Core
{
    [Serializable]
    public class ServerSettings
    {
        public List<RhinoQueueConfiguration> Listeners { get; set; }
        public RhinoQueueConfiguration Server { get; set; }
    }

    public static class ServerSettingsHelper
    {
        #region Public Methods

        public static void Save(string file, ServerSettings settings)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = File.OpenWrite(file))
            {
                new XmlSerializer(typeof(ServerSettings)).Serialize(stream, settings);
            }
        }

        public static ServerSettings Load(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return (ServerSettings)new XmlSerializer(typeof(ServerSettings)).Deserialize(stream);
            }
        }

        #endregion
    }
}
