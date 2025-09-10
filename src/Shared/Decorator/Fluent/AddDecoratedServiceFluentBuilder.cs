using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection todo) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain)
    {
        todo.AddScoped<TService>(serviceProvider =>
            ComposeDecoratedService(serviceProvider, descriptorChain));
        throw new System.NotImplementedException();
    }
}
