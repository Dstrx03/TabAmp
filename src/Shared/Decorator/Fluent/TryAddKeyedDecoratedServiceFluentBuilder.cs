using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class TryAddKeyedDecoratedServiceFluentBuilder<TService, TImplementation>(
    IServiceCollection serviceCollection,
    object? serviceKey) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Transient(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddKeyedTransient(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }

    internal override IServiceCollection Scoped(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddKeyedScoped(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }

    internal override IServiceCollection Singleton(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }
}
