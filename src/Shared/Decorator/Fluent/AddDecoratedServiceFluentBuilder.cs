using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder<TService> : DecoratedServiceFluentBuilder<TService>
    where TService : notnull
{
    internal override IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain)
    {
        throw new System.NotImplementedException();
    }
}
