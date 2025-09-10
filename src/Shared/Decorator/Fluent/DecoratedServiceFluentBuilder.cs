using System;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public abstract class DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>();
        return new(this, [descriptor]);
    }

    internal abstract IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain);

    private protected static TService ComposeDecoratedService(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorNode<TService> descriptorChain)
    {
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
