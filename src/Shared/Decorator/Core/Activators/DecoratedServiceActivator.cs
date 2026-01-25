using System;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Core.DisposableContainer;

namespace TabAmp.Shared.Decorator.Core.Activators;

internal static class DecoratedServiceActivator
{
    internal static TService CreateService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(descriptorChain);

        var service = ResolveImplementationService<TService, TImplementation>(serviceProvider, descriptorChain);
        ServiceDecoratorDisposableContainer<TService>? disposableContainer = null;

        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            CreateDecorator(service: ref service,
                disposableContainer: ref disposableContainer,
                descriptor: descriptor,
                serviceProvider: serviceProvider,
                descriptorChain: descriptorChain);

            descriptor = descriptor.Next;
        }

        return disposableContainer?.DecorateService(decoratedService: service) ?? service;
    }

    private static void CreateDecorator<TService>(
        ref TService service,
        ref ServiceDecoratorDisposableContainer<TService>? disposableContainer,
        ServiceDecoratorDescriptorChain<TService> descriptor,
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        var decorator = descriptor.CreateDecorator(serviceProvider, service);

        var isInner = descriptor.Next is not null;
        var isDisposable = descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable;

        if (isDisposable && (isInner || disposableContainer != null))
        {
            disposableContainer ??= ResolveDisposableContainer(descriptorChain);
            disposableContainer.CaptureDisposableDecorator(serviceDecorator: decorator);
        }

        service = decorator;
    }

    private static TService ResolveImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        if (!descriptorChain.UseStandaloneImplementationService)
            return ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);

        return serviceProvider.GetRequiredKeyedService<TImplementation>(serviceKey: descriptorChain.ImplementationServiceKey);
    }

    private static ServiceDecoratorDisposableContainer<TService> ResolveDisposableContainer<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        if (!descriptorChain.IsDisposableContainerAllowed)
            throw DisposableContainerIsNotAllowedException(typeof(TService));

        return ServiceDecoratorDisposableContainer<TService>.Create(descriptorChain);
    }

    private static InvalidOperationException DisposableContainerIsNotAllowedException(Type serviceType) =>
        new($"Unable to activate decorated type '{serviceType.FullName}'. " +
            "At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed.");
}
