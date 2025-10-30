using TabAmp.Shared.Decorator.Fluent;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class DescriptorChain
{
    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> For<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull, TService => new();
}
