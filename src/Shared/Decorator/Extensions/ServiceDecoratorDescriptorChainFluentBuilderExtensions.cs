namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderExtensions
{
    public static bool IsEmpty<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.IsEmpty;

    public static bool IsSingle<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.IsSingle;

    public static bool IsNormalized<TService, TImplementation>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> builder)
        where TService : notnull
        where TImplementation : notnull, TService => builder.IsNormalized;
}
