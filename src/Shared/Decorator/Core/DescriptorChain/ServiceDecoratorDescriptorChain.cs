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

    internal sealed class RootNode<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next),
        IServiceDecoratorDescriptorChainMetadata
        where TDecorator : notnull, TService
    {
        public object? ImplementationServiceKey { get; }

        public RootNode(ServiceDecoratorDescriptorChain<TService>? next, object? implementationServiceKey) :
            this(next) => ImplementationServiceKey = implementationServiceKey ?? CreateDefaultImplementationServiceKey();

        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);

        private object CreateDefaultImplementationServiceKey() => this;
    }
}
