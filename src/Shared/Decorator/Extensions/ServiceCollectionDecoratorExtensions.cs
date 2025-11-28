using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Activators;
using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddScoped(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddSingleton(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddKeyedTransient(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddKeyedScoped(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.AddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddScoped(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddSingleton(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddKeyedTransient(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddKeyedScoped(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
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

        return configureDescriptorChain(DescriptorChainConfiguration.For<TService, TImplementation>());
    }
}
