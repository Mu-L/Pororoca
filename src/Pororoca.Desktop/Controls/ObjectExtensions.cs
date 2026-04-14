using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Extensions for all types.
/// </summary>
public static class ObjectExtensions
{
    // Base class of adapter of weak event handler.
    abstract class BaseWeakEventHandlerAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TObject, THandler> : IDisposable where THandler : Delegate
    {
        // Fields.
        readonly EventInfo eventInfo;
        readonly THandler handlerStub;
        readonly WeakReference<THandler> handlerRef;
        int isDisposed;
        readonly SynchronizationContext? syncContext;
        readonly TObject target;

        // Constructor.
        protected BaseWeakEventHandlerAdapter(TObject target, string eventName, THandler handler)
        {
            this.eventInfo = typeof(TObject).GetEvent(eventName) ?? throw new ArgumentException($"Cannot find event '{eventName}' in {typeof(TObject).Name}.");
            // ReSharper disable VirtualMemberCallInConstructor
            this.handlerStub = this.CreateEventHandlerStub();
            // ReSharper restore VirtualMemberCallInConstructor
            this.handlerRef = new(handler);
            this.syncContext = SynchronizationContext.Current;
            this.target = target;
            this.eventInfo.AddEventHandler(target, this.handlerStub);
        }

        // Called to create stub of event handler.
        protected abstract THandler CreateEventHandlerStub();

        // Dispose.
        public void Dispose()
        {
            if (Interlocked.Exchange(ref this.isDisposed, 1) != 0)
                return;
            if (this.syncContext != null && this.syncContext != SynchronizationContext.Current)
            {
                try
                {
                    this.syncContext.Post(_ => this.eventInfo.RemoveEventHandler(target, this.handlerStub), null);
                    return;
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                { }
                // ReSharper restore EmptyGeneralCatchClause
            }
            this.eventInfo.RemoveEventHandler(target, this.handlerStub);
        }

        // Invoke event handler.
        protected void InvokeEventHandler(params object?[] args)
        {
            if (this.handlerRef.TryGetTarget(out var handler))
                handler.DynamicInvoke(args);
            else
                this.Dispose();
        }
    }

    // Adapter of weak event handler.
    class WeakEventHandlerAdapter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TObject, TArgs>(TObject target, string eventName, EventHandler<TArgs> handler) : BaseWeakEventHandlerAdapter<TObject, EventHandler<TArgs>>(target, eventName, handler)
        where TArgs : EventArgs
    {
        /// <inheritdoc/>.
        protected override EventHandler<TArgs> CreateEventHandlerStub() =>
            this.OnEventReceived;

        // Entry of event handler.
        void OnEventReceived(object? sender, TArgs e) =>
            this.InvokeEventHandler(sender, e);
    }

    /// <summary>
    /// Add weak event handler.
    /// </summary>
    /// <param name="target"><see cref="object"/>.</param>
    /// <param name="eventName">Name of event.</param>
    /// <param name="handler">Event handler.</param>
    /// <returns><see cref="IDisposable"/> which represents added weak event handler. You can call <see cref="IDisposable.Dispose"/> to remove weak event handler explicitly.</returns>
    public static IDisposable AddWeakEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TObject, TArgs>(this TObject target, string eventName, EventHandler<TArgs> handler) where TArgs : EventArgs =>
        new WeakEventHandlerAdapter<TObject, TArgs>(target, eventName, handler);
}