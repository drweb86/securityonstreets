using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace HDE.IpCamClientServer.Server.ServerC.View
{
    static class ViewHelper
    {
        #region Public Methods

        public static void TakeMdiScreenshots(this Form mdiView)
        {
            if (!mdiView.IsMdiContainer)
            {
                throw new ApplicationException("mdiView is not MDI container.");
            }

            var tempFolder = Path.Combine(Path.GetTempPath(), "Screenshots", DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss.fff", CultureInfo.CurrentCulture));
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            foreach (var window in mdiView.MdiChildren)
            {
                TakeScreenshot(window, RemoveUnsupportedPathCharacters(Path.Combine(tempFolder, string.Format("{0}.bmp", window.Text))));
            }

            Process.Start("explorer.exe", string.Format("\"{0}\"", tempFolder));
        }

        #endregion

        #region Private Methods

        private static void TakeScreenshot(Form form, string destinationFile)
        {
            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            using (var destinationBitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(destinationBitmap, new Rectangle(0, 0, destinationBitmap.Width, destinationBitmap.Height));
                destinationBitmap.Save(destinationFile);
            }
        }

        private static string RemoveUnsupportedPathCharacters(string path)
        {
            var directory = Path.GetDirectoryName(path);

            foreach (var charToRemove in Path.GetInvalidPathChars())
            {
                directory = directory.Replace(charToRemove, '_');
            }

            var name = Path.GetFileName(path);
            foreach (var charToRemove in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(charToRemove, '_');
            }

            return Path.Combine(directory, name);
        }

        #endregion
    }
}
