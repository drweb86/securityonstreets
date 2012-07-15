using System;
using System.IO;
using HDE.IpCamClientServer.Client.Commands;
using HDE.IpCamClientServer.Client.Model;
using HDE.IpCamClientServer.Client.View;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Client.Controller
{
    public class ClientController : IDisposable
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
            Log = new SimpleFileLog(Path.Combine(
                    Path.GetTempPath(),
                    @"HDE\IpCamClientServer.Client"
                    ));
            Log.Open();
            _uiFactory.Register<IMainFormView, MainForm>();
        }

        #endregion

        #region Commands

        public void LoadSettings(IMainFormView view)
        {
            new LoadSettingsCmd().LoadSettings(this, view);
        }

        public void Execute()
        {
            new ExecuteCmd().Execute(this);
        }

        public void TearDown()
        {
            new TearDownCmd().TearDown(this);
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

        #region IDisposable

        public void Dispose()
        {
            if (Log != null &&
                Log.IsOpened)
            {
                Log.Close();
            }
            Log = null;
        }

        #endregion
    }
}
