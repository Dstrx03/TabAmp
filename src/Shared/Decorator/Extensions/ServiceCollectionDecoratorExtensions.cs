using System;
using TabAmp.Shared.Decorator.Fluent;

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
}
