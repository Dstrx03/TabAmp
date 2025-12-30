using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal class ServiceDecoratorDisposableContainer<TService> : DispatchProxy, IDisposable, IAsyncDisposable
    where TService : notnull
{
    private bool _disposed;
    private readonly List<TService> _disposableDecorators = [];

    private TService? _decoratedService;

    internal void CaptureDisposableDecorator(TService serviceDecorator)
    {
        if (!(serviceDecorator is IDisposable || serviceDecorator is IAsyncDisposable))
            return;

        _disposableDecorators.Add(serviceDecorator);
    }

    internal TService DecorateService(TService decoratedService)
    {
        _decoratedService = decoratedService;

        return (TService)(object)this;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) =>
        targetMethod.Invoke(_decoratedService, args);

    public void Dispose() => DisposeCore();
    public ValueTask DisposeAsync() => DisposeAsyncCore();

    private void DisposeCore() => throw new NotImplementedException();
    private ValueTask DisposeAsyncCore() => throw new NotImplementedException();
}
