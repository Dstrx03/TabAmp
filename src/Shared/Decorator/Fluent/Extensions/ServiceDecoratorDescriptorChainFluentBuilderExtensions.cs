using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderExtensions
{
    public static bool IsEmpty<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.IsEmpty;

    public static bool UseStandaloneImplementationService<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.UseStandaloneImplementationService;

    public static ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder,
        ServiceDecoratorDescriptor<TService>? descriptor)
        where TService : notnull
        where TImplementation : notnull, TService => builder.With(descriptor);
}
