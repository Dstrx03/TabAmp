using System;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator;

internal static class DecoratedServiceActivator
{
    internal static TService CreateService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptor<TService> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(descriptorChain);

        TService service = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.DecorateService(serviceProvider, service);
            descriptor = descriptor.Next;
        }

        return service;
    }
}
