using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AForge.Video;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.Logging;
using Hde.StreetWatch.Commands;
using Hde.StreetWatch.Model;
using Hde.StreetWatch.View;

namespace Hde.StreetWatch.Controller
{
    class SWController
    {
        #region Properties

        public SWModel Model { get; private set; }
        public QueueLog Log { get; private set; }
        public UIFactory Factory { get; private set; }

        #endregion

        #region Constructors

        public SWController(SWModel model)
        {
            Model = model;
            Factory = new UIFactory();


            Factory.Register<IViewVideo, ViewVideoForm>();
        }

        #endregion

        #region Log

        public void Initialise()
        {
            const string htmlOpen = @"<!DOCTYPE HTML PUBLIC "" -//W3C//DTD HTML 4.0 Transitional//EN"">
                <HTML>
                 <HEAD>
                 	<META HTTP-EQUIV=""CONTENT-TYPE"" CONTENT=""text/html; charset=utf-8"">
                 	<TITLE>{0} - {1}</TITLE>
                 	<META NAME=""GENERATOR"" CONTENT=""StreetWatch operations log"">
                 </HEAD>
                 <BODY>
                 <table border = ""0"" align=""center"" cellpadding=""5"" cellspacing=""0"" width=""100%"">
                 	<tr>
                 		<td align=""center"" nowrap=""nowrap"" bgcolor=""#264b99"">
                 			<a target=""_new"" href=""{2}""><font color=""#ffffff"">{5}</font></a>
                 			<a target=""_new"" href=""{3}""><font color=""#ffffff"">{6}</font></a>
                 			<a target=""_new"" href=""{4}""><font color=""#ffffff"">{7}</font></a>
                 		</td>
                 	</tr>
                 <table>
                 </br>
                </param>";

            var folder = Path.Combine(Path.GetTempPath(), @"Hde.StreetWatch");

            Log = new QueueLog(
                LogLevel.Support,
                LogMode.File,
                true,
                new FileLog(folder, LogLevel.Support, htmlOpen, true),
                new ConsoleLog(LogLevel.Support));
            Log.Open();
        }

        public void TearDown()
        {
            if (Log != null)
            {
                Log.Close();
                Log = null;
            }
        }

        #endregion

        #region Commands

        public void PlayVideo(JPEGStream jpegStream)
        {
            new PlayVideoCmd().PlayVideo(this, jpegStream)
        }

        #endregion

    }
}
