using System;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.Extensions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }

    internal bool IsDisposable { get; }
    internal bool IsAsyncDisposable { get; }

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next, Type decoratorType)
    {
        Next = next;

        IsDisposable = decoratorType.IsDisposable();
        IsAsyncDisposable = decoratorType.IsAsyncDisposable();
    }

    internal IServiceDecoratorDescriptorChainMetadata? Metadata => this as IServiceDecoratorDescriptorChainMetadata;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    internal sealed class Node<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next, typeof(TDecorator))
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }

    internal sealed class MetadataNode<TDecorator>(
        ServiceDecoratorDescriptorChain<TService>? next,
        object? implementationServiceKey) :
        ServiceDecoratorDescriptorChain<TService>(next, typeof(TDecorator)),
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
