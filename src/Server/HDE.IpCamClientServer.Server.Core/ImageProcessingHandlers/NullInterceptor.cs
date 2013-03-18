namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers
{
    public sealed class NullInterceptor: IInterceptor
    {
        public void Intercept(string difference, byte[] image)
        {
        }

        public void Intercept(string status)
        {
        }

        public void Intercept(int currentProcessed, int totalToProcess)
        {
        }
    }
}