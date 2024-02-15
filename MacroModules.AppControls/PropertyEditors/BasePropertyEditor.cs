using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MacroModules.AppControls.PropertyEditors;

public partial class BasePropertyEditor : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty PropertyNameProperty =
        DependencyProperty.Register("PropertyName", typeof(string), typeof(BasePropertyEditor), new PropertyMetadata(""));

    public string PropertyName
    {
        get { return (string)GetValue(PropertyNameProperty); }
        set { SetValue(PropertyNameProperty, value); }
    }

    public static readonly DependencyProperty PropertyDescriptionProperty =
        DependencyProperty.Register("PropertyDescription", typeof(string), typeof(BasePropertyEditor), new PropertyMetadata(""));

    public string PropertyDescription
    {
        get { return (string)GetValue(PropertyDescriptionProperty); }
        set { SetValue(PropertyDescriptionProperty, value); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}