using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;
using Flags = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainFlags;
using Options = TabAmp.Shared.Decorator.Core.DescriptorChain.ServiceDecoratorDescriptorChainOptions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
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
    internal bool UseA => HasFlag(Flags.AllowA) && (Next?.AnyDecoratorRequiresDisposal ?? false);

    internal bool IsDecoratorDisposable => HasFlag(Flags.IsDecoratorDisposable);
    internal bool IsDecoratorAsyncDisposable => HasFlag(Flags.IsDecoratorAsyncDisposable);
    internal bool DecoratorRequiresDisposal => HasFlag(Flags.DecoratorRequiresDisposal);
    internal bool AnyDecoratorRequiresDisposal => HasFlag(Flags.AnyDecoratorRequiresDisposal);

    private bool HasFlag(ServiceDecoratorDescriptorChainFlags flag) => (_flags & flag) == flag;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    private class Node<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainOptions options) :
        ServiceDecoratorDescriptorChain<TService>(next, options, typeof(TDecorator))
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    private class ImplementationServiceKeyNode<TDecorator> : Node<TDecorator>
        where TDecorator : notnull, TService
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
        where TDecorator : notnull, TService
    {
        var useDefaultImplementationServiceKey = options.HasFlag(Options.UseDefaultImplementationServiceKey);
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
        var isDecoratorDisposable = decoratorType.IsDisposable();
        var isDecoratorAsyncDisposable = decoratorType.IsAsyncDisposable();
        var decoratorRequiresDisposal = isDecoratorDisposable || isDecoratorAsyncDisposable;
        var anyDecoratorRequiresDisposal = decoratorRequiresDisposal || (next?.AnyDecoratorRequiresDisposal ?? false);
        var allowA = options.HasFlag(Options.AllowA);

        ServiceDecoratorDescriptorChainFlags flags = new();

        if (isDecoratorDisposable) flags |= Flags.IsDecoratorDisposable;
        if (isDecoratorAsyncDisposable) flags |= Flags.IsDecoratorAsyncDisposable;
        if (decoratorRequiresDisposal) flags |= Flags.DecoratorRequiresDisposal;
        if (anyDecoratorRequiresDisposal) flags |= Flags.AnyDecoratorRequiresDisposal;
        if (allowA) flags |= Flags.AllowA;

        return flags;
    }
}
