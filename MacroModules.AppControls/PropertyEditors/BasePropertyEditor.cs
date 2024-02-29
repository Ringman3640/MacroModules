﻿using CommunityToolkit.Mvvm.Messaging;
using MacroModules.AppControls.Messages;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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

    public BasePropertyEditor()
    {
        // Store derived type public properties if not already stored
        Type currentType = GetType();
        if (!typePropInfoMap.ContainsKey(currentType))
        {
            // The flags prevent the inherited properties from being included
            PropertyInfo[] propInfo = currentType.GetProperties(BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly);
            typePropInfoMap[currentType] = propInfo;
        }
    }

    public void ReloadProperties()
    {
        if (!typePropInfoMap.TryGetValue(GetType(), out PropertyInfo[]? propInfoList))
        {
            return;
        }

        foreach (PropertyInfo propInfo in propInfoList)
        {
            OnPropertyChanged(propInfo.Name);
        }
    }

    protected virtual void PropertyEditor_Loaded(object sender, RoutedEventArgs e)
    {
        ReloadProperties();
        StrongReferenceMessenger.Default.Register<ReloadPropertyEditorsMessage>(this, (recipient, message) =>
        {
            ReloadProperties();
        });
    }

    protected virtual void PropertyEditor_Unloaded(object sender, RoutedEventArgs e)
    {
        StrongReferenceMessenger.Default.Unregister<ReloadPropertyEditorsMessage>(this);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected static int GetIntFromText(string text)
    {
        text = WhitespaceSelector().Replace(text, "");
        return (text == "") ? 0 : int.Parse(text);
    }

    protected static double GetDoubleFromText(string text)
    {
        text = WhitespaceSelector().Replace(text, "");
        return (text == "") ? 0 : double.Parse(text);
    }

    [GeneratedRegex(@"\s+")]
    protected static partial Regex WhitespaceSelector();

    private static Dictionary<Type, PropertyInfo[]> typePropInfoMap = [];
}