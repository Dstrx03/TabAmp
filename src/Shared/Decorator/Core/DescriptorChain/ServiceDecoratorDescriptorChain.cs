using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;
using Flags = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainFlags;
using Options = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainOptions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : class
{
    private readonly ServiceDecoratorDescriptorChainFlags _flags;

    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }

    private ServiceDecoratorDescriptorChain(
        ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainOptions options,
        Type decoratorType)
    {
        _flags = ComposeFlags(next, options, decoratorType);
        Next = next;
    }

    internal virtual object? ImplementationServiceKey => null;

    internal bool UseStandaloneImplementationService => ImplementationServiceKey is not null;
    internal bool UsePreRegistrationValidation => !HasFlag(Flags.SkipPreRegistrationValidation);
    internal bool IsServiceDisposable => HasFlag(Flags.IsServiceDisposable);
    internal bool IsServiceAsyncDisposable => HasFlag(Flags.IsServiceAsyncDisposable);
    internal bool IsDecoratorDisposable => HasFlag(Flags.IsDecoratorDisposable);
    internal bool IsDecoratorAsyncDisposable => HasFlag(Flags.IsDecoratorAsyncDisposable);
    internal bool IsDisposableContainerAllowed => HasFlag(Flags.IsDisposableContainerAllowed);

    private bool HasFlag(ServiceDecoratorDescriptorChainFlags flag) => (_flags & flag) == flag;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    private class Node<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainOptions options) :
        ServiceDecoratorDescriptorChain<TService>(next, options, typeof(TDecorator))
        where TDecorator : class, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    private class ImplementationServiceKeyNode<TDecorator> : Node<TDecorator>
        where TDecorator : class, TService
    {
        internal override object ImplementationServiceKey { get; }

        public ImplementationServiceKeyNode(
            ServiceDecoratorDescriptorChain<TService>? next,
            ServiceDecoratorDescriptorChainOptions options,
            object? implementationServiceKey) : base(next, options)
        {
            ImplementationServiceKey = implementationServiceKey is null ? this : implementationServiceKey;
        }
    }

    internal static ServiceDecoratorDescriptorChain<TService> CreateNode<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainOptions options = default,
        object? implementationServiceKey = null)
        where TDecorator : class, TService
    {
        var useDefaultImplementationServiceKey = options.HasOption(Options.UseDefaultImplementationServiceKey);
        var useStandaloneImplementationService = implementationServiceKey is not null || useDefaultImplementationServiceKey;

        if (useStandaloneImplementationService)
        {
            if (useDefaultImplementationServiceKey)
                implementationServiceKey = null;

            return new ImplementationServiceKeyNode<TDecorator>(next, options, implementationServiceKey);
        }

        return new Node<TDecorator>(next, options);
    }

    private static ServiceDecoratorDescriptorChainFlags ComposeFlags(
        ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainOptions options,
        Type decoratorType)
    {
        var isServiceDisposable = next?.HasFlag(Flags.IsServiceDisposable) ?? typeof(TService).IsDisposable();
        var isServiceAsyncDisposable = next?.HasFlag(Flags.IsServiceAsyncDisposable) ?? typeof(TService).IsAsyncDisposable();
        var isDecoratorDisposable = decoratorType.IsDisposable();
        var isDecoratorAsyncDisposable = decoratorType.IsAsyncDisposable();
        var isDisposableContainerAllowed = options.HasOption(Options.IsDisposableContainerAllowed);
        var skipPreRegistrationValidation = false;

        ServiceDecoratorDescriptorChainFlags flags = new();

        if (isServiceDisposable) flags |= Flags.IsServiceDisposable;
        if (isServiceAsyncDisposable) flags |= Flags.IsServiceAsyncDisposable;
        if (isDecoratorDisposable) flags |= Flags.IsDecoratorDisposable;
        if (isDecoratorAsyncDisposable) flags |= Flags.IsDecoratorAsyncDisposable;
        if (isDisposableContainerAllowed) flags |= Flags.IsDisposableContainerAllowed;
        if (skipPreRegistrationValidation) flags |= Flags.SkipPreRegistrationValidation;

        return flags;
    }
}
