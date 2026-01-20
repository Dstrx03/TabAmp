using System;
using System.Reflection;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class ProxiedIAsyncDisposableServiceDecoratorDisposableContainer<TService> :
    ServiceDecoratorDisposableContainer<TService>,
    IDisposable
    where TService : class
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (IsDisposeAsyncMethodSignature(targetMethod!))
            return DisposeAsyncCore();

        return base.Invoke(targetMethod, args);
    }

    public void Dispose() => DisposeCore();
}
