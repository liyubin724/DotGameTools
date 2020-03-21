using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Dot.Tool.ProtoGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GUIEnv.ReadGUIConfig(GUIEnv.GUICONFIG_DEFAULT_PATH);
        }

        private void Application_Exit(object sender,ExitEventArgs e)
        {
            GUIEnv.WriterGUIConfig(GUIEnv.GUICONFIG_DEFAULT_PATH);
        }
    }
}
