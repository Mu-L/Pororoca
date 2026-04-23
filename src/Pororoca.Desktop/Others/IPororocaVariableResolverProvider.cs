using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.Others;

internal interface IPororocaVariableResolverProvider
{
    // IMPORTANTE: este método deve retornar um CollectionViewModel,
    // e não simplesmente uma coleção, pois senão não vai atualizar
    // as variáveis de coleção e de ambiente.
    IPororocaVariableResolver ProvideVariableResolver();
}