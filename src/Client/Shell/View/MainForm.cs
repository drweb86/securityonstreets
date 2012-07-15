using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Controller;
using HDE.Platform.AspectOrientedFramework;

namespace HDE.IpCamClientServer.Client.View
{
    public interface IMainFormView : IBaseView<ClientController>
    {
        TabControl TabControl { get; }
        MenuStrip MainMenu { get; }
    }

    public partial class MainForm : Form, IMainFormView
    {
        #region Fields

        private ClientController _controller;

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        #region IBaseView implementation

        public void SetContext(ClientController context)
        {
            _controller = context;
        }

        public bool Process()
        {
            ShowDialog();
            return true;
        }

        public TabControl TabControl
        {
            get { return _tabControl; }
        }

        public MenuStrip MainMenu
        {
            get { return _mainMenuStrip; }
        }

        #endregion

        private void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.TearDown();
        }
    }
}
