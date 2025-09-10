using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

public abstract class DecoratedServiceFluentBuilder<TService>
    where TService : notnull
{
    public ServiceDecoratorDescriptorChainFluentBuilder<TService> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Instance<TDecorator>();
        return new(this, [descriptor]);
    }

    internal abstract IServiceCollection Scoped(ServiceDecoratorDescriptorNode<TService> descriptorChain);
}
