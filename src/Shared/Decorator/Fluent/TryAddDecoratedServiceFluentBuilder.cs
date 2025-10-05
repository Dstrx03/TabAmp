using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class TryAddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Transient(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddTransient(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }

    internal override IServiceCollection Scoped(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddScoped(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }

    internal override IServiceCollection Singleton(ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        serviceCollection.TryAddSingleton(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));
        return serviceCollection;
    }
}
