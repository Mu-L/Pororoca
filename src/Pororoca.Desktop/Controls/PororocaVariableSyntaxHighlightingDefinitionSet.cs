using System.Text.RegularExpressions;
using Avalonia.Media;
using Pororoca.Domain.Features.VariableResolution;
using static Pororoca.Domain.Features.VariableResolution.PororocaPredefinedVariableEvaluator;

namespace Pororoca.Desktop.Controls;

internal sealed class PororocaVariableSyntaxHighlightingDefinitionSet : SyntaxHighlightingDefinitionSet
{
    private readonly Func<IPororocaVariableResolver> varResolverObtainer;

    internal PororocaVariableSyntaxHighlightingDefinitionSet(Func<IPororocaVariableResolver> varResolverObtainer) : base(string.Empty)
    {
        this.varResolverObtainer = varResolverObtainer;
        TokenDefinitions =
        [
            new(id: 1, name: "Pororoca Variable", IPororocaVariableResolver.PororocaVariableRegex)
            {
                RegexMatchIdMapper = MapPororocaVariableRegexMatchToId,
                RegexMatchIdForegroundMapper = MapPororocaVariableRegexMatchIdToForeground
            }
        ];
    }

    private IBrush MapPororocaVariableRegexMatchIdToForeground(int regexMatchId) => regexMatchId switch
    {
        0 => PororocaThemeManager.NoMatchingVariableForegroundBrush,
        1 => PororocaThemeManager.RegularVariableForegroundBrush,
        2 => PororocaThemeManager.PredefinedVariableForegroundBrush,
        _ => Brushes.Red
    };

    private int MapPororocaVariableRegexMatchToId(Match match)
    {
        string keyName = match.Groups["k"].Value;

        if (IsPredefinedVariable(keyName))
        {
            return 2;
        }
        else
        {
            var varResolver = this.varResolverObtainer();
            return varResolver.IsEffectiveVariable(keyName) ? 1 : 0;
        }
    }
}