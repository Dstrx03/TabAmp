using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }
    protected ServiceDecoratorDescriptorChainFlags Flags { get; }

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainFlags flags) => (Next, Flags) = (next, flags);

    internal virtual object? ImplementationServiceKey => null;
    internal bool UseStandaloneImplementationService => ImplementationServiceKey is not null;

    internal bool IsDecoratorDisposable => HasFlag(ServiceDecoratorDescriptorChainFlags.IsDecoratorDisposable);
    internal bool IsDecoratorAsyncDisposable => HasFlag(ServiceDecoratorDescriptorChainFlags.IsDecoratorAsyncDisposable);
    internal bool DecoratorRequiresDisposal => IsDecoratorDisposable || IsDecoratorAsyncDisposable;
    internal bool AnyDecoratorRequiresDisposal => HasFlag(ServiceDecoratorDescriptorChainFlags.AnyDecoratorRequiresDisposal);

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    internal sealed class Node<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next, typeof(TDecorator).ToDescriptorChainFlags(next))
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    internal sealed class MetadataNode<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        object? implementationServiceKey) :
        ServiceDecoratorDescriptorChain<TService>(next, typeof(TDecorator).ToDescriptorChainFlags(next))
        where TDecorator : notnull, TService
    {
        internal override object? ImplementationServiceKey { get; } = implementationServiceKey;

        private MetadataNode(ServiceDecoratorDescriptorChain<TService>? next)
            : this(next, null) => ImplementationServiceKey = this;

        internal static MetadataNode<TDecorator> CreateWithDefaultImplementationServiceKey(
            ServiceDecoratorDescriptorChain<TService>? next) => new(next);

        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    private bool HasFlag(ServiceDecoratorDescriptorChainFlags flag) => (Flags & flag) == flag;
}
