using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Transient(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddTransient<TService>(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));

    internal override IServiceCollection Scoped(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddScoped<TService>(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));

    internal override IServiceCollection Singleton(ServiceDecoratorDescriptor<TService> descriptorChain) =>
        serviceCollection.AddSingleton<TService>(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));
}
