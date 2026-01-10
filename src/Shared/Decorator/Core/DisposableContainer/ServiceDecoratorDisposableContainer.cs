using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TabAmp.Shared.Decorator.Core.DisposableContainer;

internal abstract class ServiceDecoratorDisposableContainer<TService> : DispatchProxy
    where TService : class
{
    private int _disposed;
    private List<TService> _disposableDecorators = [];

    private TService? _decoratedService;

    internal void CaptureDisposableDecorator(TService serviceDecorator)
    {
        if (serviceDecorator is IDisposable || serviceDecorator is IAsyncDisposable)
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
        var disposed = BeginDispose();
        if (disposed) return;

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

            _disposableDecorators[i] = null!;
        }

        _disposableDecorators = null!;
    }

    protected ValueTask DisposeAsyncCore() => throw new NotImplementedException();

    private bool BeginDispose() => Interlocked.Exchange(ref _disposed, 1) != 0;
}
