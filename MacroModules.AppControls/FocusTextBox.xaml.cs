using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MacroModules.AppControls;

public partial class FocusTextBox : UserControl
{
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        name: nameof(Text),
        propertyType: typeof(string),
        ownerType: typeof(FocusTextBox),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: "",
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public bool SelectAllOnFocus
    {
        get { return (bool)GetValue(SelectAllOnFocusProperty); }
        set { SetValue(SelectAllOnFocusProperty, value); }
    }
    public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(
        name: nameof(SelectAllOnFocus),
        propertyType: typeof(bool),
        ownerType: typeof(FocusTextBox),
        typeMetadata: new PropertyMetadata(false));

    public FocusTextBox()
    {
        InitializeComponent();
    }

    protected void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (SelectAllOnFocus)
        {
            var textbox = (TextBox)sender;
            textbox.Dispatcher.BeginInvoke(() => textbox.SelectAll());
        }
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
