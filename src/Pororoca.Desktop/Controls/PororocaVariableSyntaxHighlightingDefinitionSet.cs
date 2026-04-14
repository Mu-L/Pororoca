using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.Controls;

internal static class PororocaVariableSyntaxHighlightingDefinitionSet
{
    public static SyntaxHighlightingDefinitionSet Create()
    {
        var definitionSet = new SyntaxHighlightingDefinitionSet(name: "URL");
        definitionSet.TokenDefinitions.Add(new(name: "Pororoca Variable")
        {
            Foreground = PororocaThemeManager.RegularVariableForegroundBrush,
            Pattern = IPororocaVariableResolver.PororocaVariableRegex,
        });
        return definitionSet;
    }
}