using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public class SiarheiKuchukMovementDetector : JulioClaudioSoraiaMovementDetector
    {
        public SiarheiKuchukMovementDetector(IInterceptor interceptor) : base(interceptor)
        {
        }

        protected override void InitializeBackgroundModel(int trainingFrames, int regionFrameSizeDivided2)
        {
            _backgroundModel.Initialize(trainingFrames, regionFrameSizeDivided2, (byte)_k, true);
        }

        protected override void PreprocessFrame(byte[] dataHW, int stride, int width, int height)
        {
            if (!_backgroundModel.IsOperational())
            {
                GrayScaleImageHelper.ApplySaltAndPapperNoise(2, dataHW, stride, width, height);
            }
        }
    }
}