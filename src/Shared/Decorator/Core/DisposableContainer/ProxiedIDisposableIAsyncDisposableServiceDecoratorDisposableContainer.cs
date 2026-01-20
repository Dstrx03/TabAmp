using System.Reflection;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class ProxiedIDisposableIAsyncDisposableServiceDecoratorDisposableContainer<TService> :
    ServiceDecoratorDisposableContainer<TService>
    where TService : class
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (IsDisposeMethodSignature(targetMethod!))
        {
            DisposeCore();
            return null;
        }

        if (IsDisposeAsyncMethodSignature(targetMethod!))
            return DisposeAsyncCore();

        return base.Invoke(targetMethod, args);
    }
}
