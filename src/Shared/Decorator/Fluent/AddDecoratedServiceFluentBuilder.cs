using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public sealed class AddDecoratedServiceFluentBuilder : DecoratedServiceFluentBuilder
{
    internal override IServiceCollection Scoped(ServiceDecoratorDescriptorNode descriptorChain)
    {
        throw new System.NotImplementedException();
    }
}
