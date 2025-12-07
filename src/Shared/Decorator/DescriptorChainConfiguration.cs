using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator;

public static class DescriptorChainConfiguration
{
    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> For<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull, TService => new();
}
