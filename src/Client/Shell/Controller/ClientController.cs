using System;
using System.IO;
using HDE.IpCamClientServer.Client.Commands;
using HDE.IpCamClientServer.Client.Model;
using HDE.IpCamClientServer.Client.View;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.AspectOrientedFramework.WinForms;
using HDE.Platform.Logging;
using IMainFormView = HDE.IpCamClientServer.Client.View.IMainFormView;

namespace HDE.IpCamClientServer.Client.Controller
{
    public class ClientController : ShellBaseController<ClientModel>
    {
        #region Fields

        private readonly UIFactory _uiFactory = new UIFactory();

        #endregion

        #region Properties

        public ClientModel Model { get; private set; }
        public ILog Log { get; private set; }
        
        #endregion

        #region Constructors

        public ClientController()
        {
            Model = new ClientModel();
            _uiFactory.Register<IMainFormView, MainForm>();
        }

        #endregion

        #region Commands

        public void LoadSettings(IMainFormView view)
        {
            Configure(view);
        }

        public void Execute()
        {
            new ExecuteCmd().Execute(this);
        }

        public void TearDown()
        {
            TearDownTools();
        }

        #endregion

        #region Public Methods

        public IView CreateView<IView>()
            where IView : IBaseView<ClientController>
        {
            var type = _uiFactory.Get(typeof(IView));
            var result = (IBaseView<ClientController>)Activator.CreateInstance(type);
            result.SetContext(this);
            return (IView)result;
        }

        #endregion

        protected override ILog CreateOpenLog()
        {
            var log = new SimpleFileLog(Path.Combine(
                    Path.GetTempPath(),
                    @"HDE\IpCamClientServer.Client"
                    ));
            log.Open();
            return log;
        }
    }
}
