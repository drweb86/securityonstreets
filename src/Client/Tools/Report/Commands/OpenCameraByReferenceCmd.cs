using HDE.IpCamClientServer.Client.Report.Controller;
using HDE.IpCamClientServer.Common.AspectOrientedFramework.Services;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class OpenCameraByReferenceCmd
    {
        public void OpenCameraByReference(ReportController controller, string camReference)
        {
            ((IMessagePump)controller.CommonServices[typeof(IMessagePump)]).SendMessage(
                "Cam Viewer",
                "Activate");

            ((IMessagePump)controller.CommonServices[typeof(IMessagePump)]).SendMessage(
                "Cam Viewer",
                "OpenCameraByReference",
                camReference);
        }
    }
}
