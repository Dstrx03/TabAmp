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

        var service = ResolveImplementationService<TService, TImplementation>(serviceProvider, descriptorChain);
        var disposableContainer = ResolveDisposableContainer(descriptorChain, implementationService: service);

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

    private static TService ResolveImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        if (!descriptorChain.UseStandaloneImplementationService)
            return ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);

        return serviceProvider.GetRequiredKeyedService<TImplementation>(serviceKey: descriptorChain.ImplementationServiceKey);
    }

    private static ServiceDecoratorDisposableContainer<TService>? ResolveDisposableContainer<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        TService implementationService)
        where TService : notnull
    {
        if (!descriptorChain.UseDisposableContainer)
            return null;

        if (!descriptorChain.IsDisposableContainerAllowed)
            throw DisposableContainerIsNotAllowed(typeof(TService));

        var disposableContainer = implementationService switch
        {
            IDisposable and IAsyncDisposable => throw new NotImplementedException("TODO"),
            IDisposable => throw new NotImplementedException("TODO"),
            IAsyncDisposable => throw new NotImplementedException("TODO"),
            _ => DispatchProxy.Create<TService, ServiceDecoratorDisposableContainer<TService>>()
        };

        return (ServiceDecoratorDisposableContainer<TService>)(object)disposableContainer;
    }

    private static InvalidOperationException DisposableContainerIsNotAllowed(Type serviceType) =>
        new($"Unable to activate decorated type '{serviceType.FullName}'. " +
            "At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed.");
}
