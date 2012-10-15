using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using HDE.IpCamClientServer.Server.Core.ImageProcessingHandlers;
using Tests.View;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnImageToGrayscaleClick(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var bitmap = new Bitmap(openFileDialog.FileName))
                    {
                        if (BackgroundImage != null)
                        {
                            BackgroundImage.Dispose();
                        }
                        BackgroundImage = GrayScaleImageHelper.ToGrayScale(bitmap);
                    }
                }
            }
        }

        private void OnCalculateMeanAndVarianceClick(object sender, EventArgs e)
        {
            var baseDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            var testData = GrayScaleImageHelper.ToGrayScale(Path.Combine(baseDir, @"Test cases\MeanVarianceTest\Input.txt"));
            byte[,] meanTest;
            byte[,] varianceTest;
            GrayScaleImageHelper.CalculateMeanAndVarianceM9(testData, out meanTest, out varianceTest);
            var meanExpected = ArrayHelper.FromFile(Path.Combine(baseDir, @"Test cases\MeanVarianceTest\MeanOutput.txt"));
            var varianceExpected = ArrayHelper.FromFile(Path.Combine(baseDir, @"Test cases\MeanVarianceTest\VarianceOutput.txt"));
            if (!ArrayHelper.Compare(meanExpected, meanTest, 1))
            {
                MessageBox.Show("Mean test failed.");
            }
            if (!ArrayHelper.Compare(varianceExpected, varianceTest, 1))
            {
                MessageBox.Show("Variance test failed.");
            }
            
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var bitmap = new Bitmap(openFileDialog.FileName))
                    {
                        var grayscale = GrayScaleImageHelper.ToGrayScale(bitmap);
                        byte[,] mean;
                        byte[,] variance;
                        GrayScaleImageHelper.CalculateMeanAndVarianceM9(grayscale, out mean, out variance);
                        var viewResultForm = new ViewResultForm();
                        viewResultForm.Initialize("Average Brightness M9", GrayScaleImageHelper.ToGrayScale(mean));
                        viewResultForm.Show();

                        var viewResultForm2 = new ViewResultForm();
                        viewResultForm2.Initialize("Variance Brightness M9", GrayScaleImageHelper.ToGrayScale(variance));
                        viewResultForm2.Show();
                    }
                }
            }
        }

        private void testButton__Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var bitmap = new Bitmap(openFileDialog.FileName))
                    {
                        using (var gs = GrayScaleImageHelper.ToGrayScale(bitmap))
                        {
                            using (var gsClone = (Bitmap)gs.Clone())
                            {
                                double[,] result;
                                GrayScaleImageHelper.GetMultiplier1(gs, gsClone, out result);
                                var maxValue = double.MinValue;
                                for (int wi = 0;wi<result.GetLength(0);wi++)
                                {
                                    for (int hi = 0; hi < result.GetLength(1); hi++)
                                    {
                                        if (maxValue < result[wi,hi])
                                        {
                                            maxValue = result[wi, hi];
                                        }

                                    }
                                }

                                MessageBox.Show("" + Math.Round(maxValue));
                            }
                        }
                    }
                }
            }
        }
    }
}
