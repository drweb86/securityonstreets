using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public interface IHandler
    {
        string Process(Bitmap bitmap);
    }
}