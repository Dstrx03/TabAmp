using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator;
using TabAmp.Shared.Decorator.Fluent;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static AddDecoratedServiceFluentBuilder<TService, TImplementation> AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new AddDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection);
    }
    public static IServiceCollection AddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }
    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddScoped(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }
    public static IServiceCollection AddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddSingleton(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }





    public static AddKeyedDecoratedServiceFluentBuilder<TService, TImplementation> AddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new AddKeyedDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection, serviceKey);
    }
    public static IServiceCollection AddKeyedDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddKeyedTransient(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }
    public static IServiceCollection AddKeyedDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddKeyedScoped(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }
    public static IServiceCollection AddKeyedDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.AddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));

        return serviceCollection;
    }





    public static TryAddDecoratedServiceFluentBuilder<TService, TImplementation> TryAddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new TryAddDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection);
    }
    public static void TryAddDecoratedTransient<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.TryAddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }
    public static void TryAddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.TryAddScoped(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }
    public static void TryAddDecoratedSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(configureDescriptorChain);
        serviceCollection.TryAddSingleton(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
    }


    public static TryAddKeyedDecoratedServiceFluentBuilder<TService, TImplementation> TryAddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new TryAddKeyedDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection, serviceKey);
    }







    public delegate ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> ConfigureDescriptorChain<TService, TImplementation>(
        ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService;

    private static ServiceDecoratorDescriptor<TService> GetDescriptorChain<TService, TImplementation>(
        ConfigureDescriptorChain<TService, TImplementation> configureDescriptorChain)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(configureDescriptorChain);

        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>();
        var descriptorChain = configureDescriptorChain(builder).BuildDescriptorChain();

        return descriptorChain;
    }
}
