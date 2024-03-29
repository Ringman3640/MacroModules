﻿using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class NumberPropertyEditor : BasePropertyEditor
{
    public ValueType NumberProperty
    {
        get { return (ValueType)GetValue(NumberPropertyProperty); }
        set { SetValue(NumberPropertyProperty, value); }
    }
    public static readonly DependencyProperty NumberPropertyProperty = DependencyProperty.Register(
        name: nameof(NumberProperty),
        propertyType: typeof(ValueType),
        ownerType: typeof(NumberPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: 0,
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string NumberEntryText
    {
        get { return NumberProperty.ToString()!; }
        set
        {
            try
            {
                ValueType valueFromText = NumberProperty;
                if (NumberProperty is int)
                {
                    valueFromText = GetIntFromText(value);
                }
                else if (NumberProperty is double)
                {
                    valueFromText = GetDoubleFromText(value);
                }
                else
                {
                    throw new Exception("Binding NumberProperty is an unsupported type");
                }

                if (NumberProperty != valueFromText)
                {
                    NumberProperty = valueFromText;
                    OnPropertyChanged();
                }
            }
            catch { }
        }
    }

    public override UIElement LabelTarget { get; }

    public NumberPropertyEditor() : base()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }
}
