using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Span of syntax highlighting.
/// TODO: EXCLUIR, POIS NÃO É NECESSÁRIA
/// </summary>
public class SyntaxHighlightingSpan : SyntaxHighlightingDefinition
{
    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingSpan"/> instance.
    /// </summary>
    /// <param name="name">Name.</param>
    public SyntaxHighlightingSpan(string? name = null) : base(name)
    { }

    // Check equality of two patterns.
    private static bool ArePatternsEqual(Regex? x, Regex? y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return x.ToString() == y.ToString() && x.Options == y.Options;
    }

    /// <summary>
    /// Get or set end pattern of the span.
    /// </summary>
    public Regex? EndPattern
    {
        get;
        set
        {
            if (ArePatternsEqual(field, value))
                return;
            field = value;
            this.Validate();
            this.OnPropertyChanged(nameof(EndPattern));
        }
    }

    /// <inheritdoc/>
    protected override bool OnValidate() =>
        base.OnValidate()
        && EndPattern is not null
        && StartPattern is not null;

    /// <summary>
    /// Get or set start pattern of the span.
    /// </summary>
    public Regex? StartPattern
    {
        get;
        set
        {
            if (ArePatternsEqual(field, value))
                return;
            field = value;
            this.Validate();
            this.OnPropertyChanged(nameof(StartPattern));
        }
    }

    /// <summary>
    /// Get list of definitions of tokens inside the span.
    /// </summary>
    public ObservableCollection<SyntaxHighlightingToken> TokenDefinitions { get; } = new();

    /// <inheritdoc/>
    public override string ToString() =>
        string.IsNullOrEmpty(this.Name)
            ? $"{{{StartPattern}}}-{{{EndPattern}}}"
            : $"[{this.Name}]{{{StartPattern}}}-{{{EndPattern}}}";
}