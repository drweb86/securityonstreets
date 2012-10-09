using System.Drawing;
using System.Windows.Forms;

namespace Tests.View
{
    public partial class ViewResultForm : Form
    {
        #region Constructors

        public ViewResultForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void Initialize(string title, Bitmap result)
        {
            Text = title;
            BackgroundImage = result;
        }

        #endregion
    }
}
