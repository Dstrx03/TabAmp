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

    internal void CaptureDisposableDecorator(TService serviceDecorator) =>
        _disposableDecorators.Add(serviceDecorator);

    internal TService DecorateService(TService decoratedService)
    {
        _decoratedService = decoratedService;
        return (TService)(object)this;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) =>
        targetMethod.Invoke(_decoratedService, args);

    protected void DisposeCore()
    {
        var disposableDecorators = BeginDispose();

        if (disposableDecorators is null)
            return;

        for (var i = disposableDecorators.Count - 1; i >= 0; i--)
        {
            var decorator = disposableDecorators[i];
            disposableDecorators[i] = null!;

            if (decorator is IDisposable disposable)
            {
                disposable.Dispose();
            }
            else if (decorator is IAsyncDisposable)
            {
                throw DecoratorOnlyImplementsIAsyncDisposableException(decorator.GetType());
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    protected async ValueTask DisposeAsyncCore()
    {
        var disposableDecorators = BeginDispose();

        if (disposableDecorators is null)
            return;

        for (var i = disposableDecorators.Count - 1; i >= 0; i--)
        {
            var decorator = disposableDecorators[i];
            disposableDecorators[i] = null!;

            if (decorator is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else if (decorator is IDisposable disposable)
            {
                disposable.Dispose();
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    private List<TService>? BeginDispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return null;

        var disposableDecorators = _disposableDecorators;
        _disposableDecorators = null!;

        return disposableDecorators;
    }

    protected static bool IsDisposeMethodSignature(MethodInfo targetMethod) =>
        targetMethod.DeclaringType == typeof(IDisposable)
        && targetMethod.Name.Equals(nameof(IDisposable.Dispose), StringComparison.Ordinal)
        && targetMethod.GetParameters().Length == 0
        && targetMethod.ReturnType == typeof(void);

    private static InvalidOperationException DecoratorOnlyImplementsIAsyncDisposableException(Type decoratorType) =>
        new($"Decorator type '{decoratorType.FullName}' only implements IAsyncDisposable. " +
            "Use DisposeAsync to dispose the container.");
}
