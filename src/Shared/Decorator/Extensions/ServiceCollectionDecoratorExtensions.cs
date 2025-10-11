using System;
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

    public static IServiceCollection AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        BuilderDelegate<TService, TImplementation> todo)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptorChain = GetDescriptorChain(todo);
        serviceCollection.AddTransient(serviceProvider =>
            DecoratedServiceActivator.CreateService<TService, TImplementation>(serviceProvider, descriptorChain));
        return serviceCollection;
    }

    private static ServiceDecoratorDescriptor<TService> GetDescriptorChain<TService, TImplementation>(
        BuilderDelegate<TService, TImplementation> todo)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(todo);
        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>();
        var descriptorChain = todo(builder).BuildDescriptorChain();
        return descriptorChain;
    }

    public delegate ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> BuilderDelegate<TService, TImplementation>(ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder) where TService : class where TImplementation : class, TService;



    public static AddKeyedDecoratedServiceFluentBuilder<TService, TImplementation> AddKeyedDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new AddKeyedDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection, serviceKey);
    }

    public static TryAddDecoratedServiceFluentBuilder<TService, TImplementation> TryAddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return new TryAddDecoratedServiceFluentBuilder<TService, TImplementation>(serviceCollection);
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
}
