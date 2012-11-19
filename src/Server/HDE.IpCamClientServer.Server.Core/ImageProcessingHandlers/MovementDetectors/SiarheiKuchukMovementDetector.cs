using AForge.Imaging.Filters;

namespace HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.MovementDetectors
{
    public class SiarheiKuchukMovementDetector : JulioClaudioSoraiaMovementDetector
    {
        // 2; 3 - perfect //6 -more good //12-very good 30-bad
        private SaltAndPepperNoise _noize = new SaltAndPepperNoise(2);

        public SiarheiKuchukMovementDetector(IInterceptor interceptor) : base(interceptor)
        {
        }

        protected override void PreprocessFrame(System.Drawing.Bitmap bitmap)
        {
            if (!_backgroundModel.IsOperational())
            {
                _noize.ApplyInPlace(bitmap);
            }
        }
    }
}