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
        name: nameof(PositionProperty),
        propertyType: typeof(Position),
        ownerType: typeof(PositionPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: default(Position),
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string EntryTextX
    {
        get { return PositionProperty.X.ToString(); }
        set
        {
            int parsedXPos;
            try
            {
                parsedXPos = GetIntFromText(value);
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
                parsedYPos = GetIntFromText(value);
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

    public string AccessibleNameX
    {
        get { return positionPropertyEditor.AccessibleName + " X Component"; }
    }

    public string AccessibleNameY
    {
        get { return positionPropertyEditor.AccessibleName + " Y Component"; }
    }

    public override UIElement LabelTarget { get; }

    public PositionPropertyEditor() : base()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }
}
