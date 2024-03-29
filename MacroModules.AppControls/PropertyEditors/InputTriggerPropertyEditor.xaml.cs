﻿using CommunityToolkit.Mvvm.Messaging;
using MacroModules.AppControls.Messages;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution;
using MacroModules.Model.GolbalSystems;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class InputTriggerPropertyEditor : BasePropertyEditor
{
    public InputTrigger? InputTriggerProperty
    {
        get { return (InputTrigger?)GetValue(InputTriggerPropertyProperty); }
        set { SetValue(InputTriggerPropertyProperty, value); }
    }
    public static readonly DependencyProperty InputTriggerPropertyProperty = DependencyProperty.Register(
        name: nameof(InputTriggerProperty),
        propertyType: typeof(InputTrigger),
        ownerType: typeof(InputTriggerPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: null,
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string InputTriggerDisplay
    {
        get
        {
            if (InputTriggerProperty is null)
            {
                return "No trigger set";
            }
            string text = "";
            if (InputTriggerProperty.Modifiers.HasFlag(InputModifiers.ShiftHeld)) {
                text += "SHIFT + ";
            }
            if (InputTriggerProperty.Modifiers.HasFlag(InputModifiers.CtrlHeld)) {
                text += "CTRL + ";
            }
            if (InputTriggerProperty.Modifiers.HasFlag(InputModifiers.AltHeld)) {
                text += "ALT + ";
            }
            if (Enum.IsDefined(typeof(InputCode), InputTriggerProperty.InputKeyCode))
            {
                return text + ((InputCode)InputTriggerProperty.InputKeyCode).ToString();
            }
            return text + "Vk_" + InputTriggerProperty.InputKeyCode;
        }
    }

    public override UIElement LabelTarget { get; }

    public InputTriggerPropertyEditor()
    {
        InitializeComponent();
        LabelTarget = labelTarget;
    }

    private void SetTrigger_Click(object sender, RoutedEventArgs e)
    {
        TriggerInputObtainer.InputHandler = new Action<InputTrigger>((InputTrigger) =>
        {
            InputTriggerProperty = InputTrigger;
            OnPropertyChanged(nameof(InputTriggerDisplay));
        });
        TriggerInputObtainer.Start();
    }

    protected override void PropertyEditor_Unloaded(object sender, RoutedEventArgs e)
    {
        base.PropertyEditor_Unloaded(sender, e);
        TriggerInputObtainer.Stop();
    }
}
