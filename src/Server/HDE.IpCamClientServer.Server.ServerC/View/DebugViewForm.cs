using System.Drawing;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    partial class DebugViewForm : Form
    {
        #region Properties

        private readonly int _windowNo;

        #endregion

        #region Constructors

        public DebugViewForm(int windowNo)
        {
            InitializeComponent();
            _windowNo = windowNo;
        }

        #endregion

        #region Private Methods

        private readonly object _sync = new object();
        public void Update(byte[] image)
        {
            lock (_sync)
            {
                if (BackgroundImage != null)
                {
                    Image imageOld = BackgroundImage;
                    BackgroundImage = null;
                    imageOld.Dispose();
                }
                Image imageImg = ImageHelper.FromBytes(image);

                if (Width < (imageImg.Width + 30) ||
                    Height < (imageImg.Height + 30))
                {
                    Width = imageImg.Width + 30;
                    Left = (_windowNo % 2) * Width;

                    Height = imageImg.Height + 30;
                    Top = (_windowNo / 2) * Height;
                }

                BackgroundImage = imageImg;
            }
            Application.DoEvents();
        }

        #endregion
    }
}
