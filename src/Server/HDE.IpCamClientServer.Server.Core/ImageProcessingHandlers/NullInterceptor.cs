using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public sealed class NullInterceptor: IInterceptor
    {
        public void Intercept(string difference, byte[] image)
        {
        }
    }
}