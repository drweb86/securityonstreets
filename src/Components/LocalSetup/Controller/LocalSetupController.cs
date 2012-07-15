using System;
using System.IO;
using HDE.Platform.Logging;
using LocalSetup.Commands;
using LocalSetup.Model;

namespace LocalSetup.Controller
{
    class LocalSetupController : IDisposable
    {
        #region Properties

        public LocalSetupModel Model { get; private set; }
        public ILog Log { get; private set; }

        #endregion
        
        #region Constructors

        public LocalSetupController()
        {
            Model = new LocalSetupModel();

            Log = new SimpleFileLog(
                Path.Combine(
                    Path.Combine(
                        Path.GetTempPath(), 
                        "HDE"),
                    "Local Setup"));

            Log.Open();
        }

        #endregion

        #region Commands

        public void Setup(LocalSetupSettings settings)
        {
            new SetupCmd().Setup(this, settings);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            if (Log != null)
            {
                Log.Close();
                Log = null;
            }
        }

        #endregion
    }
}
