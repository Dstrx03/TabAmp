using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator;

public static class ConfigureDescriptorChain
{
    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> For<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService => new();
}
