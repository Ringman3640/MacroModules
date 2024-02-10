using CommunityToolkit.Mvvm.ComponentModel;
using MacroModules.App.Behaviors;
using System.Windows;

namespace MacroModules.App.ViewModels
{
    public class MouseAwareVM : ObservableObject, INotifyMousePositionRequested
    {
        public Point MousePosition
        {
            get
            {
                MousePositionRequested?.Invoke(this, EventArgs.Empty);
                return _mousePosition;
            }
            set { _mousePosition = value; }
        }
        private Point _mousePosition;

        public MouseAwareBehavior MouseAwareBehaviorConnector
        {
            set { value?.SetRequestEvent(this); }
        }

        public event MousePositionRequestedHandler? MousePositionRequested;
    }
}
