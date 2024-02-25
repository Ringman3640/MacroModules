using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MacroModules.AppControls.Buttons;

public partial class LargeIconButton : UserControl
{
    public object IconContent
    {
        get { return (object)GetValue(IconContentProperty); }
        set { SetValue(IconContentProperty, value); }
    }
    public static readonly DependencyProperty IconContentProperty = DependencyProperty.Register(
        name: nameof(IconContent),
        propertyType: typeof(object),
        ownerType: typeof(LargeIconButton),
        typeMetadata: new PropertyMetadata(default(object)));

    public string ButtonText
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set { SetValue(ButtonTextProperty, value); }
    }
    public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
        name: nameof(ButtonText),
        propertyType: typeof(string),
        ownerType: typeof(LargeIconButton),
        typeMetadata: new PropertyMetadata(default(string)));

    public ICommand ButtonCommand
    {
        get { return (ICommand)GetValue(ButtonCommandProperty); }
        set { SetValue(ButtonCommandProperty, value); }
    }
    public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(
        name: nameof(ButtonCommand),
        propertyType: typeof(ICommand),
        ownerType: typeof(LargeIconButton),
        typeMetadata: new PropertyMetadata(default(ICommand)));

    public LargeIconButton()
    {
        InitializeComponent();
    }
}
