using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public interface IInterceptor
    {
        void Intercept(string key, byte[] image);
        
        void Intercept(string status);
        void Intercept(int currentProcessed, int totalToProcess);
    }
}