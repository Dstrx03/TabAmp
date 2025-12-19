using System;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Core.Decorators;
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
        var a = GetA(descriptorChain);
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.CreateDecorator(serviceProvider, service);
            a?.TODO1(service, descriptor);
            descriptor = descriptor.Next;
        }

        if (a is not null)
            service = a.TODO2(service);

        return service;
    }

    private static TService GetImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        if (!descriptorChain.UseStandaloneImplementationService)
            return ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);

        return serviceProvider.GetRequiredKeyedService<TImplementation>(serviceKey: descriptorChain.ImplementationServiceKey);
    }

    private static A<TService>? GetA<TService>(ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
    {
        if (!descriptorChain.UseA)
            return null;

        return new();
    }
}
