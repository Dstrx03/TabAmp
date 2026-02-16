using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderMethodsExtensions
{
    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceDecoratorDescriptor<TService>? descriptor)
        where TService : class
        where TImplementation : class, TService => builder.With(descriptor);

    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> AllowDisposableContainer<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.AllowDisposableContainer();

    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> SkipPreRegistrationValidation<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.SkipPreRegistrationValidation();

    public static ServiceDecoratorDescriptorChain<TService, TImplementation> BuildDescriptorChain<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.BuildDescriptorChain();
}
