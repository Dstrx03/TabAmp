using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Core.DisposableContainer;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator.Core.Activators;

internal static class DecoratedServiceActivator
{
    internal static TService CreateService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(descriptorChain);

        TService? service = null;

        ServiceDecoratorDisposableContainer<TService>? disposableContainer = null;
        TService? firstDisposableDecorator = null;

        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            CreateDecorator(service: ref service,
                disposableContainer: ref disposableContainer,
                firstDisposableDecorator: ref firstDisposableDecorator,
                descriptor: descriptor,
                serviceProvider: serviceProvider,
                descriptorChain: descriptorChain);

            descriptor = descriptor.Next;
        }

        service ??= CreateImplementationService<TService, TImplementation>(serviceProvider);
        return disposableContainer?.DecorateService(decoratedService: service) ?? service;
    }

    private static void CreateDecorator<TService, TImplementation>(
        ref TService? service,
        ref ServiceDecoratorDisposableContainer<TService>? disposableContainer,
        ref TService? firstDisposableDecorator,
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptor,
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        if (descriptor.ShouldSkipDecorator(serviceProvider))
            return;

        service ??= ResolveImplementationService(serviceProvider, descriptorChain);
        var decorator = descriptor.CreateDecorator(serviceProvider, service);

        var isInner = descriptor.Next is not null;
        var isDisposable = descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable;

        if (disposableContainer == null && firstDisposableDecorator != null)
            disposableContainer = ResolveDisposableContainer(descriptorChain, firstDisposableDecorator);

        if (disposableContainer != null && isDisposable /*&& (isInner || disposableContainer != null)*/)
            disposableContainer.CaptureDisposableDecorator(serviceDecorator: decorator);

        if (firstDisposableDecorator == null && isDisposable /*&& isInner*/)
            firstDisposableDecorator = decorator;

        service = decorator;
    }

    private static TService ResolveImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        if (descriptorChain.UseStandaloneImplementationService)
            return CreateStandaloneImplementationService(serviceProvider, descriptorChain);

        return CreateImplementationService<TService, TImplementation>(serviceProvider);
    }

    private static TService CreateImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider)
        where TService : class
        where TImplementation : class, TService
    {
        return ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
    }

    private static TService CreateStandaloneImplementationService<TService, TImplementation>(
        IServiceProvider serviceProvider,
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        return serviceProvider.GetRequiredKeyedService<TImplementation>(serviceKey: descriptorChain.ImplementationServiceKey);
    }

    private static ServiceDecoratorDisposableContainer<TService> ResolveDisposableContainer<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        TService firstDisposableDecorator)
        where TService : class
        where TImplementation : class, TService
    {
        if (!descriptorChain.IsServiceInterface)
            throw DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException(typeof(TService));

        if (!descriptorChain.IsDisposableContainerAllowed)
            throw DisposableContainerIsNotAllowedException(typeof(TService));

        var disposableContainerType = descriptorChain switch
        {
            { IsServiceDisposable: true, IsServiceAsyncDisposable: true } => typeof(ProxiedIDisposableIAsyncDisposableServiceDecoratorDisposableContainer<TService>),
            { IsServiceDisposable: true } => typeof(ProxiedIDisposableServiceDecoratorDisposableContainer<TService>),
            { IsServiceAsyncDisposable: true } => typeof(ProxiedIAsyncDisposableServiceDecoratorDisposableContainer<TService>),
            _ => typeof(DefaultServiceDecoratorDisposableContainer<TService>)
        };

        var disposableContainer = (ServiceDecoratorDisposableContainer<TService>)DispatchProxy.Create(typeof(TService), disposableContainerType);
        disposableContainer.CaptureDisposableDecorator(serviceDecorator: firstDisposableDecorator);

        return disposableContainer;
    }

    private static NotSupportedException DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException(Type serviceType) =>
        new($"Unable to activate decorated type '{serviceType.FullName}'. " +
            "At least one inner decorator type requires disposal, " +
            "but the decorator disposable container cannot be used " +
            "when the decorated type is not an interface.");

    private static InvalidOperationException DisposableContainerIsNotAllowedException(Type serviceType) =>
        new($"Unable to activate decorated type '{serviceType.FullName}'. " +
            "At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed. " +
            $"Use {nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>)}" +
            $".{nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>.AllowDisposableContainer)} " +
            $"({nameof(ServiceDecoratorDescriptorChainOptions)}" +
            $".{nameof(ServiceDecoratorDescriptorChainOptions.IsDisposableContainerAllowed)}) " +
            "to allow the use of a decorator disposable container.");
}
