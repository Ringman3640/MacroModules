using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
namespace MacroModules.App.Behaviors;

public class MouseAwareBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty MousePositionProperty = DependencyProperty.Register(
        name: nameof(MousePosition),
        propertyType: typeof(Point),
        ownerType: typeof(MouseAwareBehavior),
        typeMetadata: new PropertyMetadata(default(Point)));

    public Point MousePosition
    {
        get { return (Point)GetValue(MousePositionProperty); }
        set { SetValue(MousePositionProperty, value); }
    }

    protected override void OnAttached()
    {
        AssociatedObject.Loaded += AssociatedObject_Loaded;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        if (AssociatedObject.DataContext is INotifyMousePositionRequested requester)
        {
            requester.MousePositionRequested += subscribedVM_MousePositionRequested;
        }
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= AssociatedObject_Loaded;
    }

    private void subscribedVM_MousePositionRequested(object sender, EventArgs e)
    {
        MousePosition = Mouse.GetPosition(AssociatedObject);
    }
}
