using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MacroModules.AppControls.FocusedTextBoxes;

public class FocusedTextBoxBase : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(FocusedTextBoxBase),
        new FrameworkPropertyMetadata(
            "",
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    protected void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Tab)
        {
            ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            return;
        }
        if (e.Key == Key.Enter)
        {
            ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
        }
    }
}
