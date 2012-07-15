using System.Windows.Forms;
using HDE.IpCamClientServer.Client.CamViewer.Controller;
using HDE.IpCamClientServer.Client.CamViewer.Model;
using HDE.IpCamClientServer.Client.CamViewer.View;
using HDE.IpCamClientServer.Common.AspectOrientedFramework;

namespace HDE.IpCamClientServer.Client.CamViewer
{
    public class CamViewerTool : ToolBase
    {
        #region Fields

        private CamViewerController _controller;
        private TabPage _tabPage;
        private CamViewToolView _mainView;

        #endregion

        public override void ApplyChange(string subject, params object[] body)
        {
            if (_mainView != null)
            {
                _mainView.ApplyChange(subject, body);
            }
        }

        public override void Activate()
        {
            base.Activate();

            if (_controller == null)
            {
                _controller = new CamViewerController(Log, this, new CamViewerModel());
                _controller.LoadSettings();
                _tabPage = new TabPage(ToolName);
                _mainView = new CamViewToolView { Dock = DockStyle.Fill };
                _mainView.Init(_controller);
                _tabPage.Controls.Add(_mainView);
                TabControl.TabPages.Add(_tabPage);
                TabControl.SelectTab(_tabPage);
            }
            else // Select tab page.
            {
                TabControl.SelectTab(_tabPage);
            }
        }

        public override void Dispose()
        {
            if (_controller != null)
            {
                _mainView.TearDown();
                _mainView = null;
                _controller.Dispose();
                _controller = null;
                TabControl.TabPages.Remove(_tabPage);
                _tabPage = null;
                base.Dispose();
            }
        }

        protected override void OnMessageProcess(string subject, params object[] body)
        {
            switch (subject)
            {
                case "Activate":
                    if (subject == "Activate" && TabControl.SelectedTab != _tabPage)
                    {
                        Activate();
                    }
                    break;
                default:
                    _controller.ProcessMessage(subject, body);
                    break;
            }
        }
    }
}
