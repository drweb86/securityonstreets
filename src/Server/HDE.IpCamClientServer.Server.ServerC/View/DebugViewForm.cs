using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    public partial class DebugViewForm : Form, IDebugView
    {
        #region Constructors

        public DebugViewForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Initialize(string title)
        {
            Text = title;
            ShowDialog();
        }

        private readonly object _sync = new object();
        public void Update(byte[] image)
        {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => Update(image)));
                }
                else
                {
                    lock (_sync)
            {

                        if (BackgroundImage != null)
                        {
                            var imageOld = BackgroundImage;
                            BackgroundImage = null;
                            imageOld.Dispose();
                        }
                var imageImg = ImageHelper.FromBytes(image);

                    if (Width < (imageImg.Width + 30))
                    {
                        Width = imageImg.Width + 30;
                    }

                    if (Height < (imageImg.Height + 30))
                    {
                        Height = imageImg.Height + 30;
                    }

                    BackgroundImage = imageImg;
                }
            }
        }

        #endregion
    }
}
