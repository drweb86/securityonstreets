using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers.Gray;

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
                TakeScreenshot(window, 
                    RemoveUnsupportedPathCharacters(Path.Combine(tempFolder, string.Format("{0}.bmp", window.Text))),
                    RemoveUnsupportedPathCharacters(Path.Combine(tempFolder, string.Format("{0}_Inverted.bmp", window.Text))));
            }

            Process.Start("explorer.exe", string.Format("\"{0}\"", tempFolder));
        }

        #endregion

        #region Private Methods

        private static void TakeScreenshot(Form form, 
            string destinationFile,
            string destinationFileInverted)
        {
            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }

            if (File.Exists(destinationFileInverted))
            {
                File.Delete(destinationFileInverted);
            }

            using (var destinationBitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(destinationBitmap, new Rectangle(0, 0, destinationBitmap.Width, destinationBitmap.Height));
                destinationBitmap.Save(destinationFile);

                using (var grayScaleImage = GrayScaleImageHelper.ToGrayScale(destinationBitmap))
                {
                    var bounds = new Rectangle(0, 0, grayScaleImage.Width, grayScaleImage.Height);
                    BitmapData bitmapData = grayScaleImage.LockBits(bounds, ImageLockMode.ReadOnly, grayScaleImage.PixelFormat);
                    var grayScaleHW = new byte[grayScaleImage.Height * bitmapData.Stride];
                    Marshal.Copy(bitmapData.Scan0, grayScaleHW, 0, grayScaleImage.Height * bitmapData.Stride);
                    var stride = bitmapData.Stride;
                    grayScaleImage.UnlockBits(bitmapData);

                    GrayScaleImageHelper.Invert(grayScaleHW, stride, grayScaleImage.Width, grayScaleImage.Height);
                    var data = GrayScaleImageHelper.FromData2(grayScaleImage.Width, grayScaleImage.Height, stride, grayScaleHW);
                    File.WriteAllBytes(destinationFileInverted, data);
                }
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
