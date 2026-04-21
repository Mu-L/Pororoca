using System.Text.RegularExpressions;

namespace Pororoca.Domain.Features.VariableResolution;

public interface IRegexDefiner
{
    public Regex Pattern { get; }
}