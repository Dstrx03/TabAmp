using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService>(
    DecoratedServiceFluentBuilder<TService> decoratedServiceFluentBuilder,
    List<ServiceDecoratorDescriptor<TService>> descriptors)
    where TService : notnull
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>();
        return new(decoratedServiceFluentBuilder, [.. descriptors, descriptor]);
    }

    public IServiceCollection Scoped() => decoratedServiceFluentBuilder.Scoped(BuildDescriptorChain());

    private ServiceDecoratorDescriptorNode<TService> BuildDescriptorChain()
    {
        ServiceDecoratorDescriptorNode<TService> node = null!;

        for (var i = descriptors.Count - 1; i >= 0; i--)
            node = descriptors[i].ToNode(node);

        return node;
    }
}
