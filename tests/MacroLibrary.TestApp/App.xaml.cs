using MacroModules.MacroLibrary;
using System.Windows;

namespace MacroLibrary.TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // This is called just to be sure that the hooks are uninstalled
            InputMonitor.Uninstall();
        }
    }

}
