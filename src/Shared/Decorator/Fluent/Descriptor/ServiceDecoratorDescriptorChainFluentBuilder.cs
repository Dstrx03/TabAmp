using System;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    DecoratedServiceFluentBuilder<TService, TImplementation> decoratedServiceBuilder,
    ServiceDecoratorDescriptor<TService> descriptors)
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>(descriptors);
        return new(decoratedServiceBuilder, descriptor);
    }

    public IServiceCollection Transient() => decoratedServiceBuilder.Transient(BuildDescriptorChain());
    public IServiceCollection Scoped() => decoratedServiceBuilder.Scoped(BuildDescriptorChain());
    public IServiceCollection Singleton() => decoratedServiceBuilder.Singleton(BuildDescriptorChain());

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
