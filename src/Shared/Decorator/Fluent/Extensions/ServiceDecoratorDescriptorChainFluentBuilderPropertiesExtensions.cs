namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderPropertiesExtensions
{
    public static bool IsDisposableContainerAllowed<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.IsDisposableContainerAllowed;

    public static bool UsePreRegistrationValidation<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.UsePreRegistrationValidation;

    public static bool IsEmpty<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.IsEmpty;

    public static bool UseStandaloneImplementationService<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.UseStandaloneImplementationService;
}
