using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceCollectionDecoratorExtensions
{
    public static IServiceDecoratorFluentBuilder<TService> AddDecorated<TService, TImplementation>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TImplementation : class, TService
    {
        // test
        var test = new AddDecoratedServiceFluentBuilder<TService, TImplementation>().With<TImplementation>().With<TImplementation>().Scoped();
        // test

        return new ServiceDecoratorFluentBuilder<TService, TImplementation>(serviceCollection);
    }
}
