using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using LocalSetup.Controller;
using LocalSetup.Model;

namespace LocalSetup.Commands
{
    class SetupCmd
    {
        public void Setup(LocalSetupController controller, LocalSetupSettings settings)
        {
            string operation = null;
            try
            {
                operation = "IP Camera Emulator preparing";
                controller.Log.Info("{0}...", operation);
                UpdateIpCamEmuConfiguration(controller.Model.IpCamEmu_Configuration, settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }


            try
            {
                operation = "IP Camera Emulator registering";
                controller.Log.Info("{0}...", operation);
                RegisterIpCamera(settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }

            try
            {
                operation = "Prepare configuration folder";
                controller.Log.Info("{0}...", operation);

                if (Directory.Exists(controller.Model.ConfigurationFolder))
                {
                    var files = Directory.GetFiles(controller.Model.ConfigurationFolder);
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                    Directory.Delete(controller.Model.ConfigurationFolder);
                }

                Directory.CreateDirectory(controller.Model.ConfigurationFolder);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }


            try
            {
                operation = "Prepare client configuration: Report Tool";
                controller.Log.Info("{0}...", operation);
                ReplaceAndSave(controller.Model.Client_ReportToolConfig, _reportToolConfigTemplate, settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }

            try
            {
                operation = "Prepare client configuration: Cam View Tool";
                controller.Log.Info("{0}...", operation);
                ReplaceAndSave(controller.Model.Client_CamViewerToolConfig, _camViewToolConfigTemplate, settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }


            try
            {
                operation = "Prepare message router congiguration";
                controller.Log.Info("{0}...", operation);
                ReplaceAndSave(controller.Model.MessageRouter_WpfServerConfig, _messageRouterConfigTemplate, settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }

            try
            {
                operation = "Prepare console image processing server congiguration";
                controller.Log.Info("{0}...", operation);
                ReplaceAndSave(controller.Model.ImageProcessingServer_ServerCConfig, _ipsConfigTemplate, settings);
            }
            catch (Exception unhandledException)
            {
                controller.Log.Error(unhandledException);
                ShowError(operation, unhandledException);
            }

            MessageBox.Show("Configuration was created.");
            Application.OpenForms[0].DialogResult = DialogResult.OK;
            Application.OpenForms[0].Close();
        }

        private const string _reportToolConfigTemplate = @"<?xml version=""1.0""?>
<ReportToolSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Subscription>
      <Name>{Client1QueueName}</Name>
      <Host>localhost</Host>
      <Port>{ClientInputPort}</Port>
      <QueueName>{Client1QueueName}</QueueName>
  </Subscription>
</ReportToolSettings>";

        private const string _camViewToolConfigTemplate = @"<?xml version=""1.0""?>
<CamViewerSettings xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Connections>
    <CameraConnection>
      <Uri>http://localhost:{IpCameraEmulatorPort}</Uri>
      <User>{UserName}</User>
      <Password>{UserPassword}</Password>
      <Title>{IpCamName}</Title>
      <Description>{IpCamName} Camera</Description>
      <Reference>{IpCamId}</Reference>
    </CameraConnection>
  </Connections>
</CamViewerSettings>";

        private const string _messageRouterConfigTemplate = @"<?xml version=""1.0""?>
<ServerSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Listeners>
   <RhinoQueueConfiguration>
      <Name>{Client1QueueName}</Name>
      <Host>localhost</Host>
      <Port>{ClientInputPort}</Port>
      <QueueName>{Client1QueueName}</QueueName>
    </RhinoQueueConfiguration>
  </Listeners>
  <Server>
    <Name>{MessageRouterQueue}</Name>
    <Host>localhost</Host>
    <Port>{MessageRouterInputPort}</Port>
    <QueueName>{MessageRouterQueue}</QueueName>
  </Server>
</ServerSettings>";

        private const string _ipsConfigTemplate = @"<?xml version=""1.0""?>
<ServerSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <TargetQueueConfig>
    <Name>{MessageRouterQueue}</Name>
    <Host>localhost</Host>
    <Port>{MessageRouterInputPort}</Port>
    <QueueName>{MessageRouterQueue}</QueueName>
  </TargetQueueConfig>
    <CameraConnection>
      <Uri>http://localhost:{IpCameraEmulatorPort}</Uri>
      <User>{UserName}</User>
      <Password>{UserPassword}</Password>
      <Title>{IpCamName}</Title>
      <Description>{IpCamName} Camera</Description>
      <Reference>{IpCamId}</Reference>
    </CameraConnection>
</ServerSettings>";
        
        private void ReplaceAndSave(string file, string template, LocalSetupSettings settings)
        {
            File.Delete(file);

            File.WriteAllText(file, template
                .Replace("{ClientInputPort}", settings.ClientInputPort.ToString())
                .Replace("{IpCamId}", settings.IpCamId)
                .Replace("{IpCamName}", settings.IpCamName)
                .Replace("{IpCameraEmulatorPort}", settings.IpCameraEmulatorPort.ToString())
                .Replace("{MessageRouterInputPort}", settings.MessageRouterInputPort.ToString())
                .Replace("{UserPassword}", settings.UserPassword)
                .Replace("{UserName}", Environment.UserName)
                .Replace("{Client1QueueName}", "Client-1 Queue")
                .Replace("{MessageRouterQueue}", "Message Router Queue"));
        }

        #region Configuration File Templates

        private void ShowError (string operation, Exception unhandledException)
        {
            MessageBox.Show(string.Format("{0} failed: {1}", operation, unhandledException.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RegisterIpCamera(LocalSetupSettings settings)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                // we need configure firewall to allow ip camera to work
                var startProcessInfo = new ProcessStartInfo();
                startProcessInfo.Verb = "runas";
                startProcessInfo.UseShellExecute = true;
                startProcessInfo.FileName = "netsh";
                startProcessInfo.Arguments = string.Format(
                    @"http add urlacl url=http://localhost:{0}/ user={1}\{2} listen=yes",
                    settings.IpCameraEmulatorPort,
                    Environment.UserDomainName,
                    Environment.UserName);
                Process
                    .Start(startProcessInfo)
                    .WaitForExit();
            }
        }

        private void UpdateIpCamEmuConfiguration(string ipCamEmuConfiguration, LocalSetupSettings settings)
        {
            var document = new XmlDocument();
            document.Load(ipCamEmuConfiguration);

            document["Configuration"]["Servers"]["Server"]["Uri"].InnerText = string.Format("http://localhost:{0}/", settings.IpCameraEmulatorPort);
            document["Configuration"]["Servers"]["Server"]["Source"].Attributes["Name"].Value = settings.IpCamName;

            File.Delete(ipCamEmuConfiguration);
            document.Save(ipCamEmuConfiguration);
        }
        
        

        #endregion
    }
}
