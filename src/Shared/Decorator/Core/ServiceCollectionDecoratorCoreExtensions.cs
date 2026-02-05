using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator.Core;

public static class ServiceCollectionDecoratorCoreExtensions
{
    public static void AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptorChain = builder.BuildDescriptorChain();
        var descriptors = DescribeDecoratedService<TService, TImplementation>(descriptorChain, lifetime);

        Add(serviceCollection, descriptors);
    }

    public static void AddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptorChain = builder.BuildDescriptorChain();
        var descriptors = DescribeKeyedDecoratedService<TService, TImplementation>(serviceKey, descriptorChain, lifetime);

        Add(serviceCollection, descriptors);
    }

    public static void TryAddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptorChain = builder.BuildDescriptorChain();
        var descriptors = DescribeDecoratedService<TService, TImplementation>(descriptorChain, lifetime);

        TryAdd(serviceCollection, descriptors);
    }

    public static void TryAddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptorChain = builder.BuildDescriptorChain();
        var descriptors = DescribeKeyedDecoratedService<TService, TImplementation>(serviceKey, descriptorChain, lifetime);

        TryAdd(serviceCollection, descriptors);
    }

    private static void Add(IServiceCollection serviceCollection, DecoratedServiceDescriptors descriptors)
    {
        var (implementationService, decoratedService) = descriptors;

        if (implementationService is not null)
            serviceCollection.Add(implementationService);

        serviceCollection.Add(decoratedService);
    }

    private static void TryAdd(IServiceCollection serviceCollection, DecoratedServiceDescriptors descriptors)
    {
        var (implementationService, decoratedService) = descriptors;

        if (implementationService is not null)
            serviceCollection.TryAdd(implementationService);

        serviceCollection.TryAdd(decoratedService);
    }

    private static DecoratedServiceDescriptors DescribeDecoratedService<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ValidateDescriptorChain(descriptorChain);

        var implementationServiceDescriptor =
            DescribeImplementationService<TService, TImplementation>(descriptorChain, lifetime);

        var decoratedServiceDescriptor = ServiceDescriptor.Describe(
            serviceType: typeof(TService),
            implementationFactory: serviceProvider =>
                DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain),
            lifetime: lifetime);

        return new(implementationService: implementationServiceDescriptor,
            decoratedService: decoratedServiceDescriptor);
    }

    private static DecoratedServiceDescriptors DescribeKeyedDecoratedService<TService, TImplementation>(
        object? serviceKey,
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        ValidateDescriptorChain(descriptorChain);

        var implementationServiceDescriptor =
            DescribeImplementationService<TService, TImplementation>(descriptorChain, lifetime);

        var decoratedServiceDescriptor = ServiceDescriptor.DescribeKeyed(
            serviceType: typeof(TService),
            serviceKey: serviceKey,
            implementationFactory: (serviceProvider, _) =>
                DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain),
            lifetime: lifetime);

        return new(implementationService: implementationServiceDescriptor,
            decoratedService: decoratedServiceDescriptor);
    }

    private static ServiceDescriptor? DescribeImplementationService<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        if (!descriptorChain.UseStandaloneImplementationService)
            return null;

        var implementationType = typeof(TImplementation);
        return ServiceDescriptor.DescribeKeyed(
            serviceType: implementationType,
            serviceKey: descriptorChain.ImplementationServiceKey,
            implementationType: implementationType,
            lifetime: lifetime);
    }

    private static void ValidateDescriptorChain<TService>(ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        if (descriptorChain.UsePreRegistrationValidation)
            ServiceDecoratorDescriptorChainValidator.Validate(descriptorChain).ThrowIfAnyErrors();
    }

    private readonly ref struct DecoratedServiceDescriptors(
        ServiceDescriptor? implementationService,
        ServiceDescriptor decoratedService)
    {
        internal ServiceDescriptor? ImplementationService { get; } = implementationService;
        internal ServiceDescriptor DecoratedService { get; } = decoratedService;

        public void Deconstruct(out ServiceDescriptor? implementationService, out ServiceDescriptor decoratedService)
        {
            implementationService = ImplementationService;
            decoratedService = DecoratedService;
        }
    }
}
