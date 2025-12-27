using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.Decorators;

internal class A<TService> : DispatchProxy, IDisposable, IAsyncDisposable
    where TService : notnull
{
    private bool _disposed;
    private readonly List<TService> _disposables = [];

    private TService? _service;

    internal void TODO1(TService service)
    {
        if (!(service is IDisposable || service is IAsyncDisposable))
            return;

        _disposables.Add(service);
    }

    internal TService TODO2(TService service)
    {
        _service = service;

        return (TService)(object)this;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => targetMethod.Invoke(_service, args);

    public void Dispose() => DisposeCore();
    public ValueTask DisposeAsync() => DisposeAsyncCore();

    private void DisposeCore() => throw new NotImplementedException();
    private ValueTask DisposeAsyncCore() => throw new NotImplementedException();
}
