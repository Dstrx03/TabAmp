using System;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class DefaultServiceDecoratorDisposableContainer<TService> :
    ServiceDecoratorDisposableContainer<TService>,
    IDisposable,
    IAsyncDisposable
    where TService : class
{
    public void Dispose() => DisposeCore();
    public ValueTask DisposeAsync() => DisposeAsyncCore();
}
