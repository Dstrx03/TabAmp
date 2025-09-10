using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    DecoratedServiceFluentBuilder<TService, TImplementation> decoratedServiceFluentBuilder,
    List<ServiceDecoratorDescriptor<TService>> descriptors)
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>();
        return new(decoratedServiceFluentBuilder, [.. descriptors, descriptor]);
    }

    public IServiceCollection Scoped() => decoratedServiceFluentBuilder.Scoped(BuildDescriptorChain(descriptors));

    private static ServiceDecoratorDescriptorNode<TService> BuildDescriptorChain(
        List<ServiceDecoratorDescriptor<TService>> descriptors)
    {
        ServiceDecoratorDescriptorNode<TService> node = null!;

        for (var i = descriptors.Count - 1; i >= 0; i--)
            node = descriptors[i].ToNode(node);

        return node;
    }
}
