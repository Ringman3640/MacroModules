using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MacroLibrary.TestApp.Tests
{
    public partial class KeyboardInputTest : Window, INotifyPropertyChanged
    {
        private string keyInputText = "";
        public string KeyInputText
        {
            get { return keyInputText; }
            set
            {
                keyInputText = value;
                OnPropertyChanged();
            }
        }

        private bool supressInput = false;
        public bool SupressInput
        {
            get { return supressInput; }
            set
            {
                supressInput = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public KeyboardInputTest()
        {
            DataContext = this;
            InitializeComponent();
        }

        private bool returnToMenu = false;

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            returnToMenu = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputMonitor.SetInputHandler(KeyboardInputHandler);
            InputMonitor.Install();
            InputMonitor.CollectInput = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            InputMonitor.Uninstall();
            if (!returnToMenu)
            {
                Application.Current.Shutdown();
            }
        }

        private bool KeyboardInputHandler(InputData input)
        {
            if (input.Type == InputType.KeyDown)
            {
                KeyInputText = input.InputKeyCode.ToString();
                if (input.Modifiers.HasFlag(InputModifiers.ShiftHeld))
                {
                    KeyInputText += " SHIFT";
                }
                if (input.Modifiers.HasFlag(InputModifiers.CtrlHeld))
                {
                    KeyInputText += " CTRL";
                }
                if (input.Modifiers.HasFlag(InputModifiers.AltHeld))
                {
                    KeyInputText += " ALT";
                }
                
                if (supressInput)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
