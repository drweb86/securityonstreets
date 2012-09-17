using System.Drawing;
using System.Windows.Forms;

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

        public void Update(Image image)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Update(image)));
            }
            else
            {
                if (BackgroundImage != null)
                {
                    BackgroundImage.Dispose();
                }

                BackgroundImage = (Image)image.Clone();
            }
        }

        #endregion
    }
}
