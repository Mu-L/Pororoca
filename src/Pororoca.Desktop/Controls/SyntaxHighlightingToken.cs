using System.Text.RegularExpressions;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Token of syntax highlighting.
/// </summary>
public class SyntaxHighlightingToken : SyntaxHighlightingDefinition
{
    /// <summary>
    /// Get or set pattern of the token.
    /// </summary>
    public Regex? Pattern
    {
        get;
        set
        {
            if (ArePatternsEqual(field, value))
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(Pattern));
        }
    }

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingToken"/> instance.
    /// </summary>
    /// <param name="name">Name.</param>
    public SyntaxHighlightingToken(string name) : base(name)
    {
    }

    /// <inheritdoc/>
    protected override bool OnValidate() =>
        base.OnValidate() && Pattern is not null;

    /// <inheritdoc/>
    public override string ToString() =>
        string.IsNullOrEmpty(Name) ?
            $"{{{Pattern}}}" :
            $"[{Name}]{{{Pattern}}}";

    // Check equality of two patterns.
    private static bool ArePatternsEqual(Regex? x, Regex? y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return x.ToString() == y.ToString() && x.Options == y.Options;
    }
}