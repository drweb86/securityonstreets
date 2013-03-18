namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public interface IMovementDetectorBackgroundModel
    {
        void SetInterceptor(IInterceptor interceptor);
    }
}