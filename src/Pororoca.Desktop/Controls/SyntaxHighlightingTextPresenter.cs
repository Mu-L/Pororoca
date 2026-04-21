using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Threading;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// <see cref="Avalonia.Controls.Presenters.TextPresenter"/> which supports syntax highlighting.
/// </summary>
public class SyntaxHighlightingTextPresenter : Avalonia.Controls.Presenters.TextPresenter
{
    /// <summary>
    /// Property of <see cref="DefinitionSet"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlightingTextPresenter, SyntaxHighlightingDefinitionSet?> DefinitionSetProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlightingTextPresenter, SyntaxHighlightingDefinitionSet?>(nameof(DefinitionSet), t => t.SyntaxHighlighter.DefinitionSet, (t, ds) => t.SyntaxHighlighter.DefinitionSet = ds);

    /// <summary>
    /// Get or set syntax highlighting definition set.
    /// </summary>
    public SyntaxHighlightingDefinitionSet? DefinitionSet
    {
        get => SyntaxHighlighter.DefinitionSet;
        set => SyntaxHighlighter.DefinitionSet = value;
    }

    /// <summary>
    /// Get <see cref="SyntaxHighlighter"/> used by the control.
    /// </summary>
    protected SyntaxHighlighter SyntaxHighlighter { get; } = new()
    {
        TextTrimming = TextTrimming.None,
    };

    // Fields.
    private readonly Action correctCaretIndexAction;
    private bool isArranging;
    private bool isCreatingTextLayout;
    private bool isMeasuring;

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingTextPresenter"/> instance.
    /// </summary>
    public SyntaxHighlightingTextPresenter()
    {
        // setup actions
        this.correctCaretIndexAction =  new(() =>
        {
            if (SelectionStart != SelectionEnd)
                return;
            if (CaretIndex != SelectionStart)
                CaretIndex = SelectionStart;
        });

        // attach to syntax highlighter
        SyntaxHighlighter.PropertyChanged += (_, e) =>
        {
            var property = e.Property;
            if (property == SyntaxHighlighter.DefinitionSetProperty)
                RaisePropertyChanged(DefinitionSetProperty, (SyntaxHighlightingDefinitionSet?)e.OldValue, (SyntaxHighlightingDefinitionSet?)e.NewValue);
        };
        SyntaxHighlighter.TextLayoutInvalidated += (_, _) =>
        {
            if (!this.isArranging && !this.isCreatingTextLayout && !this.isMeasuring)
            {
                InvalidateTextLayout();
            }
            Dispatcher.UIThread.Post(InvalidateVisual);
        };
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size availableSize)
    {
        this.isArranging = true;
        try
        {
            SyntaxHighlighter.MaxWidth = availableSize.Width;
            SyntaxHighlighter.MaxHeight = availableSize.Height;
            return base.ArrangeOverride(availableSize);
        }
        finally
        {
            this.isArranging = false;
        }
    }

    /// <inheritdoc/>
    protected override TextLayout CreateTextLayout()
    {
        var syntaxHighlighter = SyntaxHighlighter;
        var definitionSet = SyntaxHighlighter.DefinitionSet;
        if (definitionSet is null || !definitionSet.HasValidDefinitions)
            return base.CreateTextLayout();
        this.isCreatingTextLayout = true;
        try
        {
            syntaxHighlighter.FlowDirection = FlowDirection;
            syntaxHighlighter.FontFamily = FontFamily;
            syntaxHighlighter.FontSize = FontSize;
            syntaxHighlighter.FontStretch = FontStretch;
            syntaxHighlighter.FontStyle = FontStyle;
            syntaxHighlighter.FontWeight = FontWeight;
            syntaxHighlighter.Foreground = Foreground;
            syntaxHighlighter.LetterSpacing = LetterSpacing;
            syntaxHighlighter.LineHeight = LineHeight;
            syntaxHighlighter.TextAlignment = TextAlignment;
            syntaxHighlighter.TextWrapping = TextWrapping;
            return syntaxHighlighter.CreateTextLayout();
        }
        finally
        {
            this.isCreatingTextLayout = false;
        }
    }

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        this.isMeasuring = true;
        try
        {
            SyntaxHighlighter.MaxWidth = availableSize.Width;
            SyntaxHighlighter.MaxHeight = availableSize.Height;
            return base.MeasureOverride(availableSize);
        }
        finally
        {
            this.isMeasuring = false;
        }
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        // TODO: Handle IndexOutOfRangeException
        base.OnPropertyChanged(change);
        var property = change.Property;
        if (property == PreeditTextProperty)
        {
            SyntaxHighlighter.PreeditText = change.NewValue as string;
        }
        else if (property == TextProperty)
        {
            SyntaxHighlighter.Text = change.NewValue as string;
        }
        else if (property == SelectionForegroundBrushProperty)
        {
            SyntaxHighlighter.SelectionForeground = change.NewValue as IBrush;
        }
        else if (property == SelectionStartProperty)
        {
            SyntaxHighlighter.SelectionStart = (int)change.NewValue!;
            Dispatcher.UIThread.Post(this.correctCaretIndexAction);
        }
        else if (property == SelectionEndProperty)
        {
            SyntaxHighlighter.SelectionEnd = (int)change.NewValue!;
            Dispatcher.UIThread.Post(this.correctCaretIndexAction);
        }
    }
}