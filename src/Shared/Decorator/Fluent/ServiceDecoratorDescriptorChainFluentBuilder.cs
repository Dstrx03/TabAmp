using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder(
    DecoratedServiceFluentBuilder decoratedServiceFluentBuilder,
    List<ServiceDecoratorDescriptor> descriptors)
{
    public ServiceDecoratorDescriptorChainFluentBuilder With<TDecorator>()
    {
        var descriptor = ServiceDecoratorDescriptor.Create<TDecorator>();
        return new(decoratedServiceFluentBuilder, [.. descriptors, descriptor]);
    }

    public IServiceCollection Scoped() => decoratedServiceFluentBuilder.Scoped(BuildDescriptorChain());

    private ServiceDecoratorDescriptorNode BuildDescriptorChain()
    {
        ServiceDecoratorDescriptorNode node = null!;

        for (var i = descriptors.Count - 1; i >= 0; i--)
            node = descriptors[i].ToNode(node);

        return node;
    }
}
