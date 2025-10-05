using System;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    DecoratedServiceFluentBuilder<TService, TImplementation> decoratedServiceFluentBuilder,
    ServiceDecoratorDescriptor<TService> descriptors)
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>(descriptors);
        return new(decoratedServiceFluentBuilder, descriptor);
    }

    public IServiceCollection Transient() => decoratedServiceFluentBuilder.Transient(BuildDescriptorChain());
    public IServiceCollection Scoped() => decoratedServiceFluentBuilder.Scoped(BuildDescriptorChain());
    public IServiceCollection Singleton() => decoratedServiceFluentBuilder.Singleton(BuildDescriptorChain());

    private ServiceDecoratorDescriptor<TService> BuildDescriptorChain()
    {
        ArgumentNullException.ThrowIfNull(descriptors);

        ServiceDecoratorDescriptor<TService> descriptorChain = null!;
        var descriptor = descriptors;
        while (descriptor is not null)
        {
            descriptorChain = descriptor with { Next = descriptorChain };
            descriptor = descriptor.Next;
        }

        return descriptorChain;
    }
}
