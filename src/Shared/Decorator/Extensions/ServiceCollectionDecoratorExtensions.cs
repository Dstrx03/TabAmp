using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator;
using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceCollection AddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedTransient<TService, TImplementation>(builder);
    }

    public static IServiceCollection AddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedScoped<TService, TImplementation>(builder);
    }

    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddDecoratedSingleton<TService, TImplementation>(builder);
    }

    public static IServiceCollection AddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedTransient<TService, TImplementation>(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedScoped<TService, TImplementation>(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        return serviceCollection.AddKeyedDecoratedSingleton<TService, TImplementation>(serviceKey, builder);
    }

    public static IServiceCollection AddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedTransient<TService, TImplementation>(builder);
    }

    public static void TryAddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }

    public static void TryAddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedScoped<TService, TImplementation>(builder);
    }

    public static void TryAddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddScoped(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }

    public static void TryAddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddDecoratedSingleton<TService, TImplementation>(builder);
    }

    public static void TryAddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedTransient<TService, TImplementation>(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedScoped<TService, TImplementation>(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
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
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : class
        where TImplementation : notnull, TService
    {
        var builder = GetDescriptorChainBuilder(configureDescriptorChain);
        serviceCollection.TryAddKeyedDecoratedSingleton<TService, TImplementation>(serviceKey, builder);
    }

    public static void TryAddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : class
        where TImplementation : notnull, TService
    {
        var descriptorChain = builder.BuildDescriptorChain();
        serviceCollection.TryAddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }

    public delegate ServiceDecoratorDescriptorChainFluentBuilder<TService> ConfigureDescriptorChain<TService>(
        ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : notnull;

    private static ServiceDecoratorDescriptorChainFluentBuilder<TService> GetDescriptorChainBuilder<TService>(
        ConfigureDescriptorChain<TService> configureDescriptorChain)
        where TService : notnull
    {
        ArgumentNullException.ThrowIfNull(configureDescriptorChain);

        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService>();
        return configureDescriptorChain(builder);
    }
}
