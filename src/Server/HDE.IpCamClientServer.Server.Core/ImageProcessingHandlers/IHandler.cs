using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public interface IHandler
    {
        void Configure(string configurationString);

        string Process(Bitmap bitmap);
    }
}