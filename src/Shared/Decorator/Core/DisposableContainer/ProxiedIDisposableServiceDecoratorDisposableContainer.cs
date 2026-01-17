using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class ProxiedIDisposableServiceDecoratorDisposableContainer<TService> :
    ServiceDecoratorDisposableContainer<TService>,
    IAsyncDisposable
    where TService : class
{
    private static bool IsDisposeInvoke_TODONAME(MethodInfo targetMethod)
    {
        var isName = string.Equals(targetMethod.Name, nameof(IDisposable.Dispose), StringComparison.Ordinal);
        var isRetType = targetMethod.ReturnType == typeof(void);
        var isDecType = targetMethod.DeclaringType == typeof(IDisposable);
        return isName && isRetType && isDecType;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {

        if (IsDisposeInvoke_TODONAME(targetMethod))
        {
            DisposeCore();
            return null;
        }

        //throw new NotImplementedException();

        return base.Invoke(targetMethod, args);
    }

    public ValueTask DisposeAsync() => DisposeAsyncCore();
}
