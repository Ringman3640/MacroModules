using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
namespace MacroModules.App.Behaviors;

public class MouseAwareBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty MousePositionProperty = DependencyProperty.Register(
        name: "MousePosition",
        propertyType: typeof(Point),
        ownerType: typeof(MouseAwareBehavior),
        typeMetadata: new PropertyMetadata(default(Point)));

    public Point MousePosition
    {
        get { return (Point)GetValue(MousePositionProperty); }
        set { SetValue(MousePositionProperty, value); }
    }

    public static readonly DependencyProperty MouseAwareBehaviorInstanceProperty =
        DependencyProperty.Register(
            name: "MouseAwareBehaviorInstance",
            propertyType: typeof(MouseAwareBehavior),
            ownerType: typeof(MouseAwareBehavior),
            typeMetadata: new PropertyMetadata());

    public MouseAwareBehavior MouseAwareBehaviorInstance
    {
        get { return (MouseAwareBehavior)GetValue(MouseAwareBehaviorInstanceProperty); }
        set { SetValue(MouseAwareBehaviorInstanceProperty, value); }
    }

    protected override void OnAttached()
    {
        MouseAwareBehaviorInstance = this;
    }

    public void SetRequestEvent(INotifyMousePositionRequested targetItem)
    {
        targetItem.MousePositionRequested += subscribedVM_MousePositionRequested;
        subscribedItem = targetItem;
    }

    protected override void OnDetaching()
    {
        if (subscribedItem != null)
        {
            subscribedItem.MousePositionRequested -= subscribedVM_MousePositionRequested;
            subscribedItem = null;
        }
    }

    private void subscribedVM_MousePositionRequested(object sender, EventArgs e)
    {
        MousePosition = Mouse.GetPosition(AssociatedObject);
    }

    private INotifyMousePositionRequested? subscribedItem;
}
