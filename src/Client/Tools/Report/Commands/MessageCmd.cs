using System.Windows.Forms;
using HDE.IpCamClientServer.Client.Report.Controller;

namespace HDE.IpCamClientServer.Client.Report.Commands
{
    class MessageCmd
    {
        public bool Ask(ReportController controller, string question, params object[] args)
        {
            string message;
            if (args == null || args.Length == 0)
            {
                message = question;
            }
            else
            {
                message = string.Format(question, args);   
            }
            return
                MessageBox.Show(message, "Question - Report Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes;
        }

        public void ShowError(ReportController controller, string error, params object[] args)
        {
            string message;
            if (args == null || args.Length == 0)
            {
                message = error;
            }
            else
            {
                message = string.Format(error, args);
            }
            MessageBox.Show(message, "Error - Report Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
