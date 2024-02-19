using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace MacroModules.App.Behaviors;

public class ViewAwareBehavior : Behavior<FrameworkElement>
{
    public FrameworkElement ViewRef
    {
        get { return (FrameworkElement)GetValue(BoundViewRefProperty); }
        set { SetValue(BoundViewRefProperty, value); }
    }
    public static readonly DependencyProperty BoundViewRefProperty = DependencyProperty.Register(
        name: nameof(ViewRef),
        propertyType: typeof(FrameworkElement),
        ownerType: typeof(ViewAwareBehavior),
        typeMetadata: new PropertyMetadata(default(FrameworkElement)));

    protected override void OnAttached()
    {
        ViewRef = AssociatedObject;
    }
}
