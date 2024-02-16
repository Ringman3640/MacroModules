using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace MacroModules.AppControls.PropertyEditors;

public partial class FilePathPropertyEditor : BasePropertyEditor
{
    public string FilePathProperty
    {
        get { return (string)GetValue(FilePathPropertyProperty); }
        set { SetValue(FilePathPropertyProperty, value); }
    }
    public static readonly DependencyProperty FilePathPropertyProperty = DependencyProperty.Register(
        name: nameof(FilePathProperty),
        propertyType: typeof(string),
        ownerType: typeof(FilePathPropertyEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            defaultValue: "",
            flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Filter
    {
        get { return (string)GetValue(FilterProperty); }
        set { SetValue(FilterProperty, value); }
    }
    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
        name: nameof(Filter),
        propertyType: typeof(string),
        ownerType: typeof(FilePathPropertyEditor),
        typeMetadata: new PropertyMetadata(""));

    public string FileNamePreview
    {
        get
        {
            if (FilePathProperty == "")
            {
                return "No file selected.";
            }
            if (!File.Exists(FilePathProperty))
            {
                return "Unknown file.";
            }
            return Path.GetFileName(FilePathProperty);
        }
    }

    public FilePathPropertyEditor()
    {
        InitializeComponent();
    }

    private void FilePathPropertyEditor_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (File.Exists(FilePathProperty))
        OnPropertyChanged(nameof(FileNamePreview));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        try
        {
            dialog.Filter = Filter;
        }
        catch
        {
            dialog.Filter = "";
        }

        if (dialog.ShowDialog() == true)
        {
            FilePathProperty = dialog.FileName;
            OnPropertyChanged(nameof(FileNamePreview));
        }
    }
}
