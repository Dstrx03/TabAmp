using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Core.Activators;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator.Core;

public static class ServiceCollectionDecoratorCoreExtensions
{
    public static void AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptors = DescribeDecoratedService(builder, lifetime);
        Add(serviceCollection, descriptors);
    }

    public static void AddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptors = DescribeKeyedDecoratedService(serviceKey, builder, lifetime);
        Add(serviceCollection, descriptors);
    }

    public static void TryAddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptors = DescribeDecoratedService(builder, lifetime);
        TryAdd(serviceCollection, descriptors);
    }

    public static void TryAddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        var descriptors = DescribeKeyedDecoratedService(serviceKey, builder, lifetime);
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
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        var descriptorChain = builder.BuildDescriptorChain();

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
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceLifetime lifetime)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        var descriptorChain = builder.BuildDescriptorChain();

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
        where TService : notnull
        where TImplementation : notnull, TService
    {
        if (!descriptorChain.TODO_NAME)
            return null;

        var implementationType = typeof(TImplementation);
        return ServiceDescriptor.DescribeKeyed(
            serviceType: implementationType,
            serviceKey: descriptorChain,
            implementationType: implementationType,
            lifetime: lifetime);
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
