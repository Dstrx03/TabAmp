using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class ProxiedIDisposableServiceDecoratorDisposableContainer<TService> :
    ServiceDecoratorDisposableContainer<TService>,
    IAsyncDisposable
    where TService : class
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        throw new NotImplementedException();

        return base.Invoke(targetMethod, args);
    }

    public ValueTask DisposeAsync() => DisposeAsyncCore();
}
