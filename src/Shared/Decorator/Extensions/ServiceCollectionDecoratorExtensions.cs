namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceCollection AddDecoratedScoped<TService, TImplementation>(this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        serviceCollection.AddScoped<TImplementation>();
        serviceCollection.AddScoped<TService>(serviceProvider =>
        {
            TService service = serviceProvider.GetRequiredService<TImplementation>();
            // ...
            return service;
        });
        return serviceCollection;
    }
}
