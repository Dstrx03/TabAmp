using System;
using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    internal static IServiceDecoratorFluentBuilder<TService> AddDecorated<TService, TImplementation>(this IServiceCollection serviceCollection) =>
        new ServiceDecoratorFluentBuilder<TService, TImplementation>(serviceCollection);

    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        Func<IServiceProvider, TService, TService> decoratorFactory)
        where TService : class
        where TImplementation : class, TService
    {
        serviceCollection.AddScoped<TImplementation>();
        serviceCollection.AddScoped<TService>(serviceProvider =>
            ImplementationFactory<TService, TImplementation>(serviceProvider, decoratorFactory));

        return serviceCollection;
    }

    private static TService ImplementationFactory<TService, TImplementation>(
        IServiceProvider serviceProvider,
        Func<IServiceProvider, TService, TService> decoratorFactory)
        where TImplementation : class, TService
    {
        TService service = serviceProvider.GetRequiredService<TImplementation>();
        return decoratorFactory(serviceProvider, service);
    }
}
