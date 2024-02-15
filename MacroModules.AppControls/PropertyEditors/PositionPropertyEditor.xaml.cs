using MacroModules.MacroLibrary.Types;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class PositionPropertyEditor : BasePropertyEditor
{
    public Position PositionProperty
    {
        get { return (Position)GetValue(PositionPropertyProperty); }
        set { SetValue(PositionPropertyProperty, value); }
    }

    public static readonly DependencyProperty PositionPropertyProperty = DependencyProperty.Register(
        nameof(PositionProperty),
        typeof(Position),
        typeof(PositionPropertyEditor),
        new FrameworkPropertyMetadata(
            default(Position),
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string EntryTextX
    {
        get { return PositionProperty.X.ToString(); }
        set
        {
            int parsedXPos;
            try
            {
                parsedXPos = int.Parse(value);
            }
            catch
            {
                return;
            }
            if (parsedXPos != PositionProperty.X)
            {
                PositionProperty = new(parsedXPos, PositionProperty.Y);
                OnPropertyChanged();
                OnPropertyChanged(nameof(PositionProperty));
            }
        }
    }

    public string EntryTextY
    {
        get { return PositionProperty.Y.ToString(); }
        set
        {
            int parsedYPos;
            try
            {
                parsedYPos = int.Parse(value);
            }
            catch
            {
                return;
            }
            if (parsedYPos != PositionProperty.Y)
            {
                PositionProperty = new(PositionProperty.X, parsedYPos);
                OnPropertyChanged();
                OnPropertyChanged(nameof(PositionProperty));
            }
        }
    }

    public PositionPropertyEditor()
    {
        InitializeComponent();
    }

    private void PositionPropertyEditor_Loaded(object sender, RoutedEventArgs e)
    {
        OnPropertyChanged(nameof(EntryTextX));
        OnPropertyChanged(nameof(EntryTextY));
    }
}
