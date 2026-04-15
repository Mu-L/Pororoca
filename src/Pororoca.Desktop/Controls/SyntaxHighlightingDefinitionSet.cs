using System.Collections.ObjectModel;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Set of syntax highlighting definition.
/// </summary>
public sealed class SyntaxHighlightingDefinitionSet
{
    /// <summary>
    /// Get name of the set.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Get list of token definitions.
    /// </summary>
    public SyntaxHighlightingToken[] TokenDefinitions { get; set; } = Array.Empty<SyntaxHighlightingToken>();

    /// <summary>
    /// Check whether at least one token or span definition has been added to the set or not.
    /// </summary>
    public bool HasDefinitions => TokenDefinitions.Any();

    /// <summary>
    /// Check whether at least one token or span definition in the set is valid or not.
    /// </summary>
    public bool HasValidDefinitions => TokenDefinitions.Any(x => x.IsValid);

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingDefinitionSet"/> instance.
    /// </summary>
    /// <param name="name">Name.</param>
    public SyntaxHighlightingDefinitionSet(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        $"{{{Name}}}";
}