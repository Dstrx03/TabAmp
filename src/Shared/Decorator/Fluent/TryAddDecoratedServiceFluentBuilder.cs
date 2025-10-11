using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class TryAddDecoratedServiceFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        
        return serviceCollection;
    }

    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        
        return serviceCollection;
    }

    internal override IServiceCollection (ServiceDecoratorDescriptor<TService> descriptorChain)
    {
        
        return serviceCollection;
    }
}
