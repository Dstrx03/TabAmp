using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddKeyedDecoratedServiceFluentBuilder<TService, TImplementation>(
    IServiceCollection serviceCollection,
    object? serviceKey) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Transient(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddKeyedTransient(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));

    internal override IServiceCollection Scoped(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddKeyedScoped(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));

    internal override IServiceCollection Singleton(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddKeyedSingleton(serviceKey, (serviceProvider, _) =>
            ComposeDecoratedService(serviceProvider, descriptorChain));
}
