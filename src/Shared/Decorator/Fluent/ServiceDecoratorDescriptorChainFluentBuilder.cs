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
        return default;
    }
}
