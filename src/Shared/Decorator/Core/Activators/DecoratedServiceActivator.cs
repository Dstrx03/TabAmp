using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Core.DisposableContainer;

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
        var disposableContainer = GetDisposableContainer(descriptorChain);

        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.CreateDecorator(serviceProvider, service);
            disposableContainer?.CaptureDisposableDecorator(serviceDecorator: service);

            descriptor = descriptor.Next;
        }

        if (disposableContainer is not null)
            service = disposableContainer.DecorateService(decoratedService: service);

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

    private static ServiceDecoratorDisposableContainer<TService>? GetDisposableContainer<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
    {
        if (!descriptorChain.UseDisposableContainer)
            return null;

        var disposableContainer = DispatchProxy.Create<TService, ServiceDecoratorDisposableContainer<TService>>();

        return (ServiceDecoratorDisposableContainer<TService>)(object)disposableContainer;
    }
}
