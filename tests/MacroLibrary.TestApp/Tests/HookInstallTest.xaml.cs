using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MacroLibrary.TestApp.Tests
{
    public partial class HookInstallTest : Window, INotifyPropertyChanged
    {
        private string statusText = "";
        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged();
            }
        }

        private string descriptionText = "";
        public string DescriptionText
        {
            get { return descriptionText; }
            set
            {
                descriptionText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public HookInstallTest()
        {
            base.DataContext = this;
            InitializeComponent();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const ushort VK_SPACE = 0x20;
        private bool returnToMenu = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.InvokeAsync(InstallInputMonitor);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            InputMonitor.Uninstall();
            if (!returnToMenu)
            {
                Application.Current.Shutdown();
            }
        }

        private void InstallInputMonitor()
        {
            StatusText = "Installing hooks";
            DescriptionText = "The InputMonitor is being installed";
            InputMonitor.Install();
            InputMonitor.CollectInput = true;
            Dispatcher.InvokeAsync(SendTestInput);
        }

        private void SendTestInput()
        {
            StatusText = "Sending Input";
            DescriptionText = "Sending a SPACE click and waiting for InputHandler to receive it";
            InputMonitor.SetInputHandler(InputHandler);
            InputControl.Click(VK_SPACE);
        }

        private bool testKeyPressedDown = false;
        private bool InputHandler(InputData input)
        {
            if (input.InputKeyCode == VK_SPACE)
            {
                if (input.Type == InputType.KeyDown)
                {
                    testKeyPressedDown = true;
                    return false;
                }
                if (input.Type == InputType.KeyUp && testKeyPressedDown)
                {
                    Dispatcher.InvokeAsync(ShowSucess);
                    InputMonitor.CollectInput = false;
                    return false;
                }
            }
            return true;
        }

        private void ShowSucess()
        {
            StatusText = "Sucess!";
            DescriptionText = "The input hook successfully received the SPACE click input";
            InputMonitor.Uninstall();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            returnToMenu = true;
            Close();
        }
    }
}
