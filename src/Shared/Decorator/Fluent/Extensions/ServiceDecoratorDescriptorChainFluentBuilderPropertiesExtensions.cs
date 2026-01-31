namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderPropertiesExtensions
{
    public static bool GetIsDisposableContainerAllowed<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.IsDisposableContainerAllowed;

    public static bool GetUsePreRegistrationValidation<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.UsePreRegistrationValidation;

    public static bool GetIsEmpty<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.IsEmpty;

    public static bool GetUseStandaloneImplementationService<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : class
        where TImplementation : class, TService => builder.UseStandaloneImplementationService;
}
