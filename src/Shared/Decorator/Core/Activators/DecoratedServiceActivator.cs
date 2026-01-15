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
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(descriptorChain);

        var service = ResolveImplementationService<TService, TImplementation>(serviceProvider, descriptorChain);
        ServiceDecoratorDisposableContainer<TService>? disposableContainer = null;
        //var disposableContainer = ResolveDisposableContainer(descriptorChain, implementationService: service);

        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            //service = descriptor.CreateDecorator(serviceProvider, service);
            //disposableContainer?.CaptureDisposableDecorator(serviceDecorator: service);
            CreateDecorator(ref service, ref disposableContainer, serviceProvider, descriptor);
            descriptor = descriptor.Next;
        }

        //return disposableContainer?.DecorateService(decoratedService: service) ?? service;
        return service;
    }

    private static void CreateDecorator<TService>(
        ref TService service,
        ref ServiceDecoratorDisposableContainer<TService>? disposableContainer,
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService> descriptor)
        where TService : class
    {
        var decorator = descriptor.CreateDecorator(serviceProvider, service);

        var isInner = descriptor.Next is not null;
        var isDisposable = descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable;

        if (isDisposable && (isInner || disposableContainer != null))
        {
            disposableContainer ??= ResolveDisposableContainer(descriptor);
            disposableContainer.CaptureDisposableDecorator(serviceDecorator: decorator);
        }

        if (!isInner && disposableContainer != null)
            decorator = disposableContainer.DecorateService(decoratedService: decorator);

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
        ServiceDecoratorDescriptorChain<TService> descriptor)
        where TService : class
    {
        if (!descriptor.IsDisposableContainerAllowed)
            throw DisposableContainerIsNotAllowed(typeof(TService));

        var disposableContainer = descriptor switch
        {
            { IsServiceDisposable: true, IsServiceAsyncDisposable: true } => throw new NotImplementedException("TODO"),
            { IsServiceDisposable: true } => throw new NotImplementedException("TODO"),
            { IsServiceAsyncDisposable: true } => throw new NotImplementedException("TODO"),
            _ => DispatchProxy.Create<TService, DefaultServiceDecoratorDisposableContainer<TService>>()
        };

        return (ServiceDecoratorDisposableContainer<TService>)(object)disposableContainer;
    }

    private static InvalidOperationException DisposableContainerIsNotAllowed(Type serviceType) =>
        new($"Unable to activate decorated type '{serviceType.FullName}'. " +
            "At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed.");
}
