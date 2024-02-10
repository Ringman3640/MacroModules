using System.Windows;

namespace MacroModules.App.Behaviors;

public interface IDimensionsAware
{
    public Size Dimensions { get; set; }
}
