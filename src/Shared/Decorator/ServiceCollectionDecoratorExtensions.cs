using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Activators;
using TabAmp.Shared.Decorator.DescriptorChain;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceCollection AddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedTransient(builder);
    }

    public static IServiceCollection AddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddDecorated(builder, ServiceLifetime.Transient);
        return serviceCollection;
    }

    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedScoped(builder);
    }

    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddDecorated(builder, ServiceLifetime.Scoped);
        return serviceCollection;
    }

    public static IServiceCollection AddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedSingleton(builder);
    }

    public static IServiceCollection AddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddDecorated(builder, ServiceLifetime.Singleton);
        return serviceCollection;
    }

    public static IServiceCollection AddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedTransient(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddKeyedDecorated(serviceKey, builder, ServiceLifetime.Transient);
        return serviceCollection;
    }

    public static IServiceCollection AddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedScoped(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddKeyedDecorated(serviceKey, builder, ServiceLifetime.Scoped);
        return serviceCollection;
    }

    public static IServiceCollection AddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedSingleton(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.AddKeyedDecorated(serviceKey, builder, ServiceLifetime.Singleton);
        return serviceCollection;
    }

    public static void TryAddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedTransient(builder);
    }

    public static void TryAddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddDecorated(builder, ServiceLifetime.Transient);
    }

    public static void TryAddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedScoped(builder);
    }

    public static void TryAddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddDecorated(builder, ServiceLifetime.Scoped);
    }

    public static void TryAddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedSingleton(builder);
    }

    public static void TryAddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddDecorated(builder, ServiceLifetime.Singleton);
    }

    public static void TryAddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedTransient(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddKeyedDecorated(serviceKey, builder, ServiceLifetime.Transient);
    }

    public static void TryAddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedScoped(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddKeyedDecorated(serviceKey, builder, ServiceLifetime.Scoped);
    }

    public static void TryAddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedSingleton(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        serviceCollection.TryAddKeyedDecorated(serviceKey, builder, ServiceLifetime.Singleton);
    }

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

    public delegate ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> ConfigureDescriptorChain<TService, TImplementation>(
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService;

    private static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> GetDescriptorChainBuilder<TService, TImplementation>(
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : notnull
        where TImplementation : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(configureDescriptorChain);

        var builder = DescriptorChainConfiguration.For<TService, TImplementation>();
        return configureDescriptorChain(builder);
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
