using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input.Platform;
using Pororoca.Desktop.Views;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// <see cref="Avalonia.Controls.TextBox"/> which supports syntax highlighting.
/// </summary>
public class SyntaxHighlightingTextBox : TextBox
{
    /// <inheritdoc/>
    protected override Type StyleKeyOverride => typeof(TextBox);

    /// <summary>
    /// Property of <see cref="DefinitionSet"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlightingTextBox, SyntaxHighlightingDefinitionSet?> DefinitionSetProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlightingTextBox, SyntaxHighlightingDefinitionSet?>(nameof(DefinitionSet), t => t.DefinitionSet, (t, ds) => t.DefinitionSet = ds);

    /// <summary>
    /// Get or set syntax highlighting definition set.
    /// </summary>
    public SyntaxHighlightingDefinitionSet? DefinitionSet
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(DefinitionSetProperty, ref field, value);
            this.textPresenter?.DefinitionSet = field;
        }
    }

    // Fields.
    private SyntaxHighlightingTextPresenter? textPresenter;

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingTextBox"/> instance.
    /// </summary>
    public SyntaxHighlightingTextBox()
    {
        PseudoClasses.Add(":syntaxHighlighted");
        PseudoClasses.Add(":syntaxHighlightingTextBox");
        PastingFromClipboard += async (_, e) =>
        {
            if (MainWindow.Instance!.Clipboard is IClipboard systemClipboard)
            {
                OnPastingFromClipboard(await systemClipboard.GetTextAsync());
            }
            e.Handled = true;
        };
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        this.textPresenter = e.NameScope.Find<SyntaxHighlightingTextPresenter>("PART_TextPresenter");
        this.textPresenter?.DefinitionSet = DefinitionSet;
    }

    /// <summary>
    /// Called when pasting text from clipboard
    /// </summary>
    /// <param name="text">The text from clipboard.</param>
    protected virtual void OnPastingFromClipboard(string? text)
    {
        if (text is null)
            return;

        SelectedText = AcceptsReturn ? text : RemoveLineBreaks(text);
    }

    private static string RemoveLineBreaks(string s) =>
        s.Replace("\\r\\n", string.Empty)
         .Replace("\\n", string.Empty);    
}