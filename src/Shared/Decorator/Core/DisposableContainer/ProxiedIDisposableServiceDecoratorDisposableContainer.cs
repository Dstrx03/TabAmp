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
        if (IsDisposeMethodSignature(targetMethod!))
        {
            DisposeCore();
            return null;
        }

        return base.Invoke(targetMethod, args);
    }

    public ValueTask DisposeAsync() => DisposeAsyncCore();
}
