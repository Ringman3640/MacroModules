using MacroLibrary.TestApp.Tests;
using System.Windows;

namespace MacroLibrary.TestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHookTest_Click(object sender, RoutedEventArgs e)
        {
            OpenModalWindow(new HookInstallTest());
        }

        private void btnKeyboardTest_Click(object sender, RoutedEventArgs e)
        {
            OpenModalWindow(new KeyboardInputTest());
        }

        private void btnMouseTest_Click(object sender, RoutedEventArgs e)
        {
            OpenModalWindow(new MouseInputTest());
        }

        private void OpenModalWindow(Window win)
        {
            Visibility = Visibility.Hidden;
            win.ShowDialog();
            Visibility = Visibility.Visible;
        }
    }
}