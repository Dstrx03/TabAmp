using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain) =>
        serviceCollection.AddScoped<TService>(serviceProvider => ComposeDecoratedService(serviceProvider, descriptorChain));
}
