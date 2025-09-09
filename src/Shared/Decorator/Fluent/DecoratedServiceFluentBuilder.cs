using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public abstract class DecoratedServiceFluentBuilder
{
    public ServiceDecoratorDescriptorChainFluentBuilder With<TDecorator>()
    {
        var descriptor = ServiceDecoratorDescriptor.Create<TDecorator>();
        return new(this, [descriptor]);
    }

    internal abstract IServiceCollection Scoped(ServiceDecoratorDescriptorNode descriptorChain);
}
