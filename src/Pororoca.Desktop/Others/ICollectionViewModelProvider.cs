using Pororoca.Desktop.ViewModels;

namespace Pororoca.Desktop.Others;

internal interface ICollectionViewModelProvider
{
    // IMPORTANTE: este método deve retornar um CollectionViewModel,
    // e não simplesmente uma coleção, pois senão não vai atualizar
    // as variáveis de coleção e de ambiente.
    CollectionViewModel ProvideVariableResolver();
}