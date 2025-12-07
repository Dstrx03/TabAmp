using System;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Core.Activators;

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

        var service = GetImplementationService<TService, TImplementation>(serviceProvider, descriptorChain);
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.DecorateService(serviceProvider, service);
            descriptor = descriptor.Next;
        }

        return service;
    }

    private static TService GetImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        if (descriptorChain.TODO_NAME)
            return serviceProvider.GetRequiredKeyedService<TImplementation>(serviceKey: descriptorChain);

        return ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
    }
}
