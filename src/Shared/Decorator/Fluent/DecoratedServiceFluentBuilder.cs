using System;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public abstract class DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        return new ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(this, null!).With<TDecorator>();
    }

    internal abstract IServiceCollection Transient(ServiceDecoratorDescriptor<TService> descriptorChain);
    internal abstract IServiceCollection Scoped(ServiceDecoratorDescriptor<TService> descriptorChain);
    internal abstract IServiceCollection Singleton(ServiceDecoratorDescriptor<TService> descriptorChain);

    private protected static TService ComposeDecoratedService(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);

        TService service = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.DecorateService(service, serviceProvider);
            descriptor = descriptor.Next;
        }

        return service;
    }
}
