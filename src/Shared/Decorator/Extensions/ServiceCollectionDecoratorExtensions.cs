using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceDecoratorFluentBuilder<TService> AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        return new ServiceDecoratorFluentBuilder<TService, TImplementation>(serviceCollection);
    }
}
