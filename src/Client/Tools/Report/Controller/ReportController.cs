using System;
using HDE.IpCamClientServer.Client.Report.Commands;
using HDE.IpCamClientServer.Client.Report.Model;
using HDE.IpCamClientServer.Common.AspectOrientedFramework.Collections;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.Logging;

namespace HDE.IpCamClientServer.Client.Report.Controller
{
    class ReportController : IDisposable
    {
        #region Fields

        private readonly UIFactory _uiFactory = new UIFactory();

        #endregion

        #region Properties

        public ILog Log { get; private set; }
        public ReportTool Tool { get; private set; }
        public ReportModel Model { get; private set; }
        public ReadOnlyDictionary<object, object> CommonServices { get; private set; }

        #endregion

        #region Constructors

        public ReportController(ILog log, ReportTool tool, ReportModel model, ReadOnlyDictionary<object, object> commonServices)
        {
            Log = log;
            Tool = tool;
            Model = model;
            CommonServices = commonServices;

            //_uiFactory.Register<IImagingServerProperties, ImagingServerPropertiesForm>();
        }

        #endregion

        #region Commands

        public void OpenCameraByReference(string camReference)
        {
            new OpenCameraByReferenceCmd().OpenCameraByReference(this, camReference);
        }

        public void LoadSettings()
        {
            new LoadSettingsCmd().LoadSettings(this);
        }

        public void SaveSettings()
        {
            new SaveSettingsCmd().SaveSettings(this);
        }

        public bool ConnectToServer()
        {
            return new ConnectToServerCmd().ConnectToServer(this);
        }

        public void DisconnectFromServer()
        {
            new DisconnectFromServerCmd().DisconnectFromServer(this);
        }

        public bool Ask(string question, params object[] args)
        {
            return new MessageCmd().Ask(this, question, args);
        }

        public void ShowError(string error, params object[] args)
        {
            new MessageCmd().ShowError(this, error, args);
        }

        #endregion

        #region Ui Factoru

        public IView CreateView<IView>()
            where IView : IBaseView<ReportController>
        {
            var type = _uiFactory.Get(typeof(IView));
            var result = (IBaseView<ReportController>)Activator.CreateInstance(type);
            result.SetContext(this);
            return (IView)result;
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            DisconnectFromServer();
        }

        #endregion
    }
}
