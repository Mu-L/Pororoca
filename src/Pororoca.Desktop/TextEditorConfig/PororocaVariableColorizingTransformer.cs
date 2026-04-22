using System.Drawing;
using System.Text.RegularExpressions;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.TextEditorConfig;

internal partial class PororocaVariableColorizingTransformer : DocumentColorizingTransformer
{
    private readonly Func<IPororocaVariableResolver> varResolverObtainer;

    internal PororocaVariableColorizingTransformer(Func<IPororocaVariableResolver> varResolverObtainer)
    {
        this.varResolverObtainer = varResolverObtainer;
    }

    protected override void ColorizeLine(DocumentLine line)
    {
        string lineText = CurrentContext.Document.GetText(line);
        var matches = IPororocaVariableResolver.PororocaVariableRegex.Matches(lineText);
        var varResolver = this.varResolverObtainer();
        foreach (object objM in matches)
        {
            var match = (Match)objM;
            string keyName = match.Groups["k"].Value;            

            ChangeLinePart(
                line.Offset + match.Index,
                line.Offset + match.Index + match.Length,
                visualLine =>
                {
                    bool noMatchingVar = !varResolver.IsPredefinedOrEffectiveVariable(keyName);
                    bool isPredefVar = match.ValueSpan.Contains('$');
                    // TODO: Get variable highlight color from Styles.xaml
                    visualLine.TextRunProperties.SetForegroundBrush(
                        noMatchingVar ?
                        PororocaThemeManager.NoMatchingVariableForegroundBrush :
                        isPredefVar ?
                        PororocaThemeManager.PredefinedVariableForegroundBrush :
                        PororocaThemeManager.RegularVariableForegroundBrush);
                }
            );
        }
    }
}