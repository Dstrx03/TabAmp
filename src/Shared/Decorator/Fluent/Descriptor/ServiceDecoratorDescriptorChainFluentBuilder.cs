using System;
using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    DecoratedServiceFluentBuilder<TService, TImplementation> decoratedServiceFluentBuilder,
    ImmutableList<ServiceDecoratorDescriptor<TService>> descriptors)
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>();
        return new(decoratedServiceFluentBuilder, [.. descriptors, descriptor]);
    }

    public IServiceCollection Scoped() => decoratedServiceFluentBuilder.Scoped(BuildDescriptorChain());

    private ServiceDecoratorDescriptorNode<TService> BuildDescriptorChain()
    {
        ArgumentNullException.ThrowIfNull(descriptors);

        if (descriptors.IsEmpty)
            throw DescriptorsIsEmptyException(typeof(TService));

        ServiceDecoratorDescriptorNode<TService> node = null!;
        for (var i = descriptors.Count - 1; i >= 0; i--)
            node = descriptors[i].ToNode(node);

        return node;
    }

    private static InvalidOperationException DescriptorsIsEmptyException(Type serviceType) =>
        new($"Cannot build decorator descriptor chain for the decorated type '{serviceType.FullName}'. " +
            "At least one decorator descriptor is required.");
}
