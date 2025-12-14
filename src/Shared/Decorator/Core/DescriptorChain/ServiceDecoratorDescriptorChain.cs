using System;
using TabAmp.Shared.Decorator.Core.Activators;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next) =>
        Next = next;

    internal IServiceDecoratorDescriptorChainMetadata? Metadata => this as IServiceDecoratorDescriptorChainMetadata;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    internal sealed class Node<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next)
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    internal sealed class MetadataNode<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        object? implementationServiceKey) :
        ServiceDecoratorDescriptorChain<TService>(next),
        IServiceDecoratorDescriptorChainMetadata
        where TDecorator : notnull, TService
    {
        public object? ImplementationServiceKey { get; } = implementationServiceKey;

        private MetadataNode(ServiceDecoratorDescriptorChain<TService>? next)
            : this(next, null) => ImplementationServiceKey = this;

        internal static MetadataNode<TDecorator> CreateWithDefaultImplementationServiceKey(
            ServiceDecoratorDescriptorChain<TService>? next) => new(next);

        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }
}
