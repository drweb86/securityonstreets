using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace HDE.IpCamClientServer.Common
{
    public static class SerializerHelper
    {
        public static TData DeserializeBinary<TData>(byte[] message)
        {
            using (var stream = new MemoryStream(message))
            {
                return (TData)new BinaryFormatter().Deserialize(stream);
            }
        }

        public static TData Load<TData>(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return (TData)new XmlSerializer(typeof(TData)).Deserialize(stream);
            }
        }

        public static void Save<TData>(TData data, string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var stream = File.OpenWrite(file))
            {
                new XmlSerializer(typeof(TData)).Serialize(stream, data);
            }
        }
    }
}
