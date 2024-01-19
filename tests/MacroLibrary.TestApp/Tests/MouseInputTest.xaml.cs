using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace MacroLibrary.TestApp.Tests
{
    /// <summary>
    /// Interaction logic for MouseInputTest.xaml
    /// </summary>
    public partial class MouseInputTest : Window, INotifyPropertyChanged
    {
        private string positionText = "";
        public string PositionText
        {
            get { return positionText; }
            set
            {
                positionText = value;
                OnPropertyChanged();
            }
        }

        private string buttonClickText = "";
        public string ButtonClickText
        {
            get { return buttonClickText; }
            set
            {
                buttonClickText = value;
                OnPropertyChanged();
            }
        }

        public MouseInputTest()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool returnToMenu = false;
        private bool suppressInputs = false;
        private DispatcherTimer suppressTimer = new();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputMonitor.Install();
            InputMonitor.SetInputHandler(MouseInputHandler);
            InputMonitor.FilterMouseMovements = false;
            InputMonitor.CollectInput = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InputMonitor.Uninstall();
            if (returnToMenu)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnSuppress_Click(object sender, RoutedEventArgs e)
        {
            if (!suppressInputs)
            {
                suppressInputs = true;
                suppressTimer.Interval = TimeSpan.FromSeconds(5);
                suppressTimer.Tick += new EventHandler(StopInputSuppression);
                suppressTimer.Start();
            }
        }
        private void StopInputSuppression(object? sender, EventArgs e)
        {
            suppressInputs = false;
            suppressTimer.Stop();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            returnToMenu = true;
            Close();
        }

        private const ushort VK_LBUTTON = 0x01;
        private const ushort VK_RBUTTON = 0x02;
        private const ushort VK_MBUTTON = 0x04;
        private const ushort VK_XBUTTON1 = 0x05;
        private const ushort VK_XBUTTON2 = 0x06;
        private bool MouseInputHandler(InputData input)
        {
            if (input.Type == InputType.MouseDown)
            {
                switch (input.InputKeyCode)
                {
                    case VK_LBUTTON:
                        ButtonClickText = "Left Click";
                        break;
                    case VK_RBUTTON:
                        ButtonClickText = "Right Click";
                        break;
                    case VK_MBUTTON:
                        ButtonClickText = "Middle Click";
                        break;
                    case VK_XBUTTON1:
                        ButtonClickText = "X1 Click";
                        break;
                    case VK_XBUTTON2:
                        ButtonClickText = "X2 Click";
                        break;
                    default:
                        ButtonClickText = $"Unknown Click {input.InputKeyCode}";
                        break;
                }
            }
            if (input.Type == InputType.MouseMove)
            {
                Position pos = MouseControl.GetCursorPosition();
                PositionText = $"({pos.X}, {pos.Y})";
            }
            if (suppressInputs)
            {
                return false;
            }
            return true;
        }

        
    }
}
