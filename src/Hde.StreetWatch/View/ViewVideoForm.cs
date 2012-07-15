using System;
using System.Drawing;
using System.Windows.Forms;
using HDE.Platform.AspectOrientedFramework;
using Hde.StreetWatch.Controller;

namespace Hde.StreetWatch.View
{
    interface IViewVideo: IBaseView<SWController>
    {
        void DisplayFrame(Bitmap frame);
        void Finish();
    }

    partial class ViewVideoForm : Form, IViewVideo
    {
        #region Fields

        private SWController _controller;

        #endregion

        #region Constructors

        public ViewVideoForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        public void SetContext(SWController context)
        {
            _controller = context;
        }

        public bool Process()
        {
            Application.Run(this);
            return true;
        }

        public void DisplayFrame(Bitmap frame)
        {
            lock(_viewPictureBox)
            {
                _viewPictureBox.Image = frame;
            }
        }

        public void Finish()
        {
            Close();
        }

        #endregion
    }
}
