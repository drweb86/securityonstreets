using System;
using System.Globalization;
using HDE.IpCamClientServer.Server.ServerC.Controller;

namespace HDE.IpCamClientServer.Server.ServerC
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var controller = new ServerController())
            {
                controller.LoadSettings();
                controller.Start();


                Console.WriteLine("Image processing server is started.");
                Console.WriteLine("Press '<Enter>' to quit.");
                Console.ReadLine();

                controller.Stop();
            }
        }
    }
}
