namespace Pororoca.Desktop.Controls;

/// <summary>
/// Empty implementation of <see cref="IDisposable"/>.
/// </summary>
public class EmptyDisposable : IDisposable
{
    /// <summary>
    /// Default instance.
    /// </summary>
    public static readonly IDisposable Default = new EmptyDisposable();

    /// <inheritdoc/>
    public void Dispose()
    { }
}