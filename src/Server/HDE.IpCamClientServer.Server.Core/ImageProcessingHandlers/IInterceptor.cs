using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public interface IInterceptor
    {
        void Intercept(string key, Image image);
    }
}