using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using HDE.IpCamClientServer.Client.Controller;
using HDE.IpCamClientServer.Client.View;
using HDE.IpCamClientServer.Common.AspectOrientedFramework;
using HDE.IpCamClientServer.Common.AspectOrientedFramework.Collections;
using HDE.IpCamClientServer.Common.AspectOrientedFramework.Services;

namespace HDE.IpCamClientServer.Client.Commands
{
    class LoadSettingsCmd
    {
        public void LoadSettings(ClientController controller, IMainFormView view)
        {
            var commonServices = new Dictionary<object, object>();
            commonServices.Add(typeof(IMessagePump), new MessagePump());
            var commonServicesAssign = commonServices.ToReadonlyDictionary();

            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var configFile = Path.Combine(binFolder, "HDE.IpCamClientServer.Client.xml");
            var shellConfig = new XmlDocument();
            shellConfig.Load(configFile);

            var toolConfigs = shellConfig.SelectNodes(@"Configuration/Tools/Load");
            foreach (XmlNode toolConfig in toolConfigs)
            {
                var activatorInfo = toolConfig
                    .Attributes["tool"].Value
                    .Split(
                        new[] {", "},
                        StringSplitOptions.RemoveEmptyEntries);

                var tool = (ITool)Activator.CreateInstance(activatorInfo[1], activatorInfo[0]).Unwrap();
                controller.Model.Tools.Add(tool);

                tool.Assign(controller.Log, 
                    toolConfig.Attributes["name"].Value,
                    view.TabControl, 
                    view.MainMenu,
                    commonServicesAssign);

                var menuPaths = toolConfig.Attributes["addToMenu"].Value.Split(new[] {'/'});
                var rootItemCollection = view.MainMenu.Items;
                for (int i = 0; i < menuPaths.Length;i++ )
                {
                    ToolStripMenuItem menu = null;
                    foreach (ToolStripMenuItem item in rootItemCollection)
                    {
                        if (item.Text == menuPaths[i])
                        {
                            menu = item;
                        }
                    }

                    if (menu == null)
                    {
                        menu = new ToolStripMenuItem(menuPaths[i]);
                        menu.Name = menuPaths[i];

                        rootItemCollection.Add(menu);
                    }
                    rootItemCollection = menu.DropDownItems;

                    if (i == menuPaths.Length - 1)
                    {
                        menu.Click += (s, e) => tool.Activate();
                    }
                }
            }
        }
    }
}
