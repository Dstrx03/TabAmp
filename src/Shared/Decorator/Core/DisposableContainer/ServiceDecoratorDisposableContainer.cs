using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal abstract class ServiceDecoratorDisposableContainer<TService> : DispatchProxy
    where TService : class
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

    protected void DisposeCore()
    {
        if (_disposed)
            return;

        _disposed = true;

        for (var i = _disposableDecorators.Count - 1; i >= 0; i--)
        {
            if (_disposableDecorators[i] is IDisposable disposable)
            {
                disposable.Dispose();
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    protected ValueTask DisposeAsyncCore() => throw new NotImplementedException();
}
