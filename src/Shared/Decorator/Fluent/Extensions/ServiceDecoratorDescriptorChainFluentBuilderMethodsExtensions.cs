using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderMethodsExtensions
{
    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceDecoratorDescriptor<TService>? descriptor)
        where TService : notnull
        where TImplementation : notnull, TService => builder.With(descriptor);

    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> AllowDisposableContainer<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.AllowDisposableContainer();
}
