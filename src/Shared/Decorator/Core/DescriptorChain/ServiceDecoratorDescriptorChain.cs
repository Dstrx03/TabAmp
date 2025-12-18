using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    private readonly ServiceDecoratorDescriptorChainFlags _flags;

    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next,
        ServiceDecoratorDescriptorChainFlags flags) => (Next, _flags) = (next, flags);

    internal virtual object? ImplementationServiceKey => null;

    internal bool UseStandaloneImplementationService => ImplementationServiceKey is not null;
    internal bool UseA => HasFlag(ServiceDecoratorDescriptorChainFlags.AllowA) && AnyDecoratorRequiresDisposal;

    internal bool IsDecoratorDisposable => HasFlag(ServiceDecoratorDescriptorChainFlags.IsDecoratorDisposable);
    internal bool IsDecoratorAsyncDisposable => HasFlag(ServiceDecoratorDescriptorChainFlags.IsDecoratorAsyncDisposable);
    internal bool DecoratorRequiresDisposal => HasFlag(ServiceDecoratorDescriptorChainFlags.DecoratorRequiresDisposal);
    internal bool AnyDecoratorRequiresDisposal => HasFlag(ServiceDecoratorDescriptorChainFlags.AnyDecoratorRequiresDisposal);

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    internal sealed class Node<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next, typeof(TDecorator).ToDescriptorChainFlags(next))
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    internal sealed class HeadNode<TDecorator> : ServiceDecoratorDescriptorChain<TService>
        where TDecorator : notnull, TService
    {
        internal override object? ImplementationServiceKey { get; }

        public HeadNode(ServiceDecoratorDescriptorChain<TService>? next,
            object? implementationServiceKey = null,
            ServiceDecoratorDescriptorChainOptions options = default)
            : base(next, typeof(TDecorator).ToDescriptorChainFlags(next, options))
        {
            ImplementationServiceKey = options.GetImplementationServiceKey(implementationServiceKey, this);
        }

        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    private bool HasFlag(ServiceDecoratorDescriptorChainFlags flag) => (_flags & flag) == flag;
}
