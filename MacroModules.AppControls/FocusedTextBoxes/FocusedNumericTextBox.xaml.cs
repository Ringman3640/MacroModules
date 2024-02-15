using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MacroModules.AppControls.FocusedTextBoxes;

public partial class FocusedNumericTextBox : FocusedTextBoxBase
{
    public FocusedNumericTextBox()
    {
        InitializeComponent();
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = numericRegex().IsMatch(e.Text);
    }

    [GeneratedRegex(@"[^0-9]+")]
    private static partial Regex numericRegex();
}
