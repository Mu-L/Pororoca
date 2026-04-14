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
    /// Property of <see cref="IsMaxTokenCountReached"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlightingTextBox, bool> IsMaxTokenCountReachedProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlightingTextBox, bool>(nameof(IsMaxTokenCountReached), t => t.IsMaxTokenCountReached);
    /// <summary>
    /// Property of <see cref="MaxTokenCount"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlightingTextBox, int> MaxTokenCountProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlightingTextBox, int>(nameof(MaxTokenCount), t => t.MaxTokenCount, (t, count) => t.MaxTokenCount = count);

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
            if (this.textPresenter is not null)
                this.textPresenter.DefinitionSet = field;
        }
    }

    /**
     * Check whether maximum number of token to be highlighted reached or not.
     */
    public bool IsMaxTokenCountReached => this.textPresenter?.IsMaxTokenCountReached ?? false;

    /// <summary>
    /// Get or set maximum number of token should be highlighted. Negative value if there is no limitation.
    /// </summary>
    public int MaxTokenCount
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(MaxTokenCountProperty, ref field, value);
            if (this.textPresenter is not null)
                this.textPresenter.MaxTokenCount = field;
        }
    } = -1;

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
        if (this.textPresenter is not null && this.textPresenter.IsMaxTokenCountReached)
        {
            RaisePropertyChanged(IsMaxTokenCountReachedProperty, true, false);
        }
        base.OnApplyTemplate(e);
        this.textPresenter = e.NameScope.Find<SyntaxHighlightingTextPresenter>("PART_TextPresenter");
        if (this.textPresenter is not null)
        {
            this.textPresenter.DefinitionSet = DefinitionSet;
            this.textPresenter.MaxTokenCount = MaxTokenCount;
            this.textPresenter.PropertyChanged += (_, e) =>
            {
                if (e.Property == SyntaxHighlightingTextPresenter.IsMaxTokenCountReachedProperty)
                {
                    RaisePropertyChanged(IsMaxTokenCountReachedProperty, (bool)e.OldValue!, (bool)e.NewValue!);
                }
            };
            if (this.textPresenter.IsMaxTokenCountReached)
            {
                RaisePropertyChanged(IsMaxTokenCountReachedProperty, false, true);
            }
        }
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