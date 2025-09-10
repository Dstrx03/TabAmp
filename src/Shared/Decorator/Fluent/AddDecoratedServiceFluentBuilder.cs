using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService, TImplementation> :
    DecoratedServiceFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    internal override IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain)
    {
        throw new System.NotImplementedException();
    }
}
