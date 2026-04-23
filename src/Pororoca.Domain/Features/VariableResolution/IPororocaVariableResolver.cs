using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Pororoca.Domain.Features.Entities.Pororoca;
using static Pororoca.Domain.Features.VariableResolution.PororocaPredefinedVariableEvaluator;

namespace Pororoca.Domain.Features.VariableResolution;

public partial interface IPororocaVariableResolver
{
    public static readonly Regex PororocaVariableRegex = GeneratePororocaVariableRegex();

    [GeneratedRegex("\\{\\{\\s*(?<k>\\$?[\\w\\d_\\-\\.]+)\\s*\\}\\}")]
    private static partial Regex GeneratePororocaVariableRegex();

    List<PororocaVariable> Variables { get; } // collection variables
    PororocaRequestAuth? CollectionScopedAuth { get; } // collection scoped auth
    List<PororocaEnvironment> Environments { get; } // collection environments

    public IEnumerable<PororocaVariable> GetEffectiveVariables()
    {
        var currentEnv = Environments.FirstOrDefault(e => e.IsCurrent);
        var effectiveEnvVars = currentEnv?.Variables?.Where(sev => sev.Enabled) ?? Enumerable.Empty<PororocaVariable>();
        var effectiveColVars = Variables.Where(cv => cv.Enabled);
        var effectiveColVarsNotInEnv =
            effectiveColVars.Where(ecv => !effectiveEnvVars.Any(eev => eev.Key == ecv.Key));

        // Environment variable overrides collection variable
        return effectiveEnvVars.Concat(effectiveColVarsNotInEnv);
    }

    [ExcludeFromCodeCoverage(Justification = "Method is too simple.")]
    public bool IsEffectiveVariable(string keyName) =>
        GetEffectiveVariables().Any(v => v.Key == keyName);

    public static Dictionary<string, string> ResolveKeyValueParams(IEnumerable<PororocaKeyValueParam>? kvParams, IEnumerable<PororocaVariable> effectiveVars) =>
        kvParams == null ?
        new() :
        kvParams.Where(h => h.Enabled)
                .Select(h => new KeyValuePair<string, string>(
                    ReplaceTemplates(h.Key, effectiveVars),
                    ReplaceTemplates(h.Value, effectiveVars)
                ))
                .DistinctBy(h => h.Key) // Avoid duplicated pairs by key
                .ToDictionary(h => h.Key, h => h.Value);

    // Example of templated string:
    // "https://{{MyApiHost}}/api/location?city=Campinas"
    // If there is a variable with the key "MyApiHost" and with the value "www.api.com.br" (all without quotes)
    // Then this method should return:
    // "https://www.api.com.br/api/location?city=Campinas"
    // The variable resolution depends on collection and environment variables.
    // Environment variables have precedence over collection variables.
    // If the variable key is not declared or the variable is not enabled, then the raw key should be used as is.

    public static string ReplaceTemplates(string? strToReplaceTemplatedVariables, IEnumerable<PororocaVariable> effectiveVars, int recursionDepth = 0)
    {
        if (string.IsNullOrEmpty(strToReplaceTemplatedVariables) || recursionDepth >= 4)
        {
            return strToReplaceTemplatedVariables ?? string.Empty;
        }
        else if (!effectiveVars.Any() && !strToReplaceTemplatedVariables.Contains('$'))
        {
            // no need to run regex replacer if there are no effective variables and no predefined variables
            return strToReplaceTemplatedVariables;
        }
        else
        {
            return PororocaVariableRegex.Replace(strToReplaceTemplatedVariables, match =>
            {
                string keyName = match.Groups["k"].Value;
                if (IsPredefinedVariable(keyName, resolveValue: true, out string? predefinedVarValue))
                {
                    return predefinedVarValue!;
                }
                else
                {
                    var effectiveVar = effectiveVars.FirstOrDefault(v => v.Key == keyName);
                    if (effectiveVar == null)
                    {
                        return match.Value;
                    }

                    return ReplaceTemplates(effectiveVar.Value, effectiveVars, recursionDepth + 1);
                }
            });
        }
    }

    // Este método só é usado no Pororoca.Desktop,
    // porém está na camada de Domain porque é lógica pesada
    // e queremos testes unitários nele.
    public static string? GetPointerHoverVariable(string? lineText, int pointerIndex)
    {
        int startIndex = -1, endIndex = -1;

        if (string.IsNullOrWhiteSpace(lineText) || lineText.Length < 5)
        {
            return null; // no mínimo "{{x}}"
        }
        if (pointerIndex == lineText.Length)
        {
            return null; // ponteiro sobre o final da linha ('\n')
        }

        if (pointerIndex <= (lineText.Length - 2) && lineText[pointerIndex] == '{' && lineText[pointerIndex + 1] == '{')
        {
            startIndex = pointerIndex;
        }
        else
        {
            for (int i = pointerIndex; i >= 1; i--)
            {
                // o i <= (pointerIndex - 2) é para aceitar caractér de fechamento
                // no caso de o mouse estar em cima da dupla de fechamento ("}}")
                if ((i <= (pointerIndex - 2)) && lineText[i] == '}')
                {
                    // encontrou caractér de fechamento ('}')
                    // antes de encontrar dupla de abertura ("{{")
                    break;
                }
                if (lineText[i] == '{' && lineText[i - 1] == '{')
                {
                    startIndex = i - 1;
                    break;
                }
            }
        }

        if (startIndex == -1)
        {
            return null;
        }

        if (pointerIndex >= 1 && lineText[pointerIndex] == '}' && lineText[pointerIndex - 1] == '}')
        {
            endIndex = pointerIndex;
        }
        else
        {
            for (int i = pointerIndex; i <= lineText.Length - 2; i++)
            {
                // o i >= (pointerIndex + 2) é para aceitar caractér de abertura
                // no caso de o mouse estar em cima da dupla de abertura ("{{")
                if ((i >= (pointerIndex + 2)) && lineText[i] == '{')
                {
                    // encontrou caractér de abertura ('{')
                    // antes de encontrar dupla de fechamento ("}}")
                    break;
                }
                if (lineText[i] == '}' && lineText[i + 1] == '}')
                {
                    endIndex = i + 1;
                    break;
                }
            }
        }

        if (endIndex == -1)
        {
            return null;
        }

        string hoveringVar = lineText[startIndex..(endIndex + 1)];

        var regexMatch = PororocaVariableRegex.Match(hoveringVar);
        return regexMatch.Success ? hoveringVar : null;
    }

    // Este método só é usado no Pororoca.Desktop,
    // porém está na camada de Domain porque é lógica pesada
    // e queremos testes unitários nele.
    public static List<(T? Pattern, int Start, int Length, Match? Match)> DelimitTextPartsOverRegexes<T>(IEnumerable<T> patternDefinitions, string? input)
        where T : class, IRegexDefiner
    {
        if (String.IsNullOrEmpty(input))
        {
            return [];
        }

        var list = patternDefinitions.SelectMany(d =>
        {
            var matches = d.Pattern.Matches(input);
            return matches.Select(m => ((T?)d, m.Index, m.Length, (Match?)m));
        }).OrderBy(x => x.Index).ToList();


        if (list.Count == 0)
        {
            return [(null!, 0, input.Length, null)];
        }
        else
        {
            // First match begins after the start of the string.
            // Let's mark this part with null.
            if (list[0].Index > 0)
            {
                list.Insert(0, (null, 0, list[0].Index, null));
            }

            for (int i = 0; i < list.Count - 1; i++)
            {
                var (_, currentStart, currentLength, currentMatch) = list[i];
                var (_, nextStart, nextLength, nextMatch) = list[i + 1];

                // This means that there is some space between 
                // the current match and the next match.
                // Let's mark this part with null.
                int currentToNextLength = nextStart - (currentStart + currentLength);
                if (currentToNextLength > 0)
                {
                    list.Insert(i + 1, (null, currentStart + currentLength, currentToNextLength, null));
                    i++; // no need to check the newly created part again
                }
            }

            // Last match ends before the end of the string.
            // Let's mark this part with null.
            int endOfLastMatch = list[^1].Index + list[^1].Length;
            if (endOfLastMatch < input.Length)
            {
                list.Add((null, endOfLastMatch, input.Length - endOfLastMatch, null));
            }

            return list;
        }
    }
}