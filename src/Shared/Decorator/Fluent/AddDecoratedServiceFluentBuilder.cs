using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain) =>
        

    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain) =>
        

    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain) =>
        
}
