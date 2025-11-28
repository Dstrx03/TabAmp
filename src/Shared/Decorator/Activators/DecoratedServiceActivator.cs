using System;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.DescriptorChain;

namespace TabAmp.Shared.Decorator.Activators;

internal static class DecoratedServiceActivator
{
    internal static TService CreateService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
        where TImplementation : notnull, TService
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
