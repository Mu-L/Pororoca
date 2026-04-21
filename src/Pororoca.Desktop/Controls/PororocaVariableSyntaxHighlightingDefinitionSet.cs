using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.Controls;

internal sealed class PororocaVariableSyntaxHighlightingDefinitionSet : SyntaxHighlightingDefinitionSet
{
    internal static readonly PororocaVariableSyntaxHighlightingDefinitionSet Singleton = new();

    private PororocaVariableSyntaxHighlightingDefinitionSet() : base(string.Empty) =>
        TokenDefinitions =
        [
            //if (Application.Current is Avalonia.Application app)
            //    brush.Bind(SolidColorBrush.ColorProperty, app, "Color/SyntaxHighlighter.SyntaxError.Underline");
            //else
            new(name: "Pororoca User Variable", IPororocaVariableResolver.PororocaUserVariableRegex)
            {
                Foreground = PororocaThemeManager.RegularVariableForegroundBrush,
            },
            new(name: "Pororoca Predefined Variable", IPororocaVariableResolver.PororocaPredefinedVariableRegex)
            {
                Foreground = PororocaThemeManager.PredefinedVariableForegroundBrush,
            }
        ];
}