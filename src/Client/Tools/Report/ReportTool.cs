using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Report.Controller;
using HDE.IpCamClientServer.Client.Report.Model;
using HDE.IpCamClientServer.Client.Report.View;
using HDE.IpCamClientServer.Common.AspectOrientedFramework;

namespace HDE.IpCamClientServer.Client.Report
{
    class ReportTool : ToolBase
    {
        #region Fields

        private ReportController _controller;
        private TabPage _tabPage;
        private ReportToolView _mainView;

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
                _controller = new ReportController(Log, this, new ReportModel(), CommonServices);
                _controller.LoadSettings();
                _tabPage = new TabPage(ToolName);
                _mainView = new ReportToolView { Dock = DockStyle.Fill };
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
    }
}
