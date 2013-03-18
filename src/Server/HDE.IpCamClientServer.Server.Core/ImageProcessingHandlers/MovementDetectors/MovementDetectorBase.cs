using System.Drawing;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public abstract class MovementDetectorBase<TBackgroundModel>: IHandler
        where TBackgroundModel : class, IMovementDetectorBackgroundModel, new()
    {
        #region Fields

        private int _skipStartingFrames;
        protected readonly IInterceptor _interceptor;
        protected TBackgroundModel _backgroundModel;

        #endregion

        public MovementDetectorBase(IInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public virtual void Configure(string configurationString)
        {
            _backgroundModel = new TBackgroundModel();
            _backgroundModel.SetInterceptor(_interceptor);

            _skipStartingFrames = 10;
        }

        public abstract string[] GetDebugWindows();
        public string Process(Bitmap bitmap)
        {
            // When video device initializes it adapts to 
            // background for some frames.
            if (_skipStartingFrames > 0)
            {
                _skipStartingFrames--;
                bitmap.Dispose();
                return null;
            }
            return ProcessInternal(bitmap);
        }

        protected abstract string ProcessInternal(Bitmap bitmap);
    }
}