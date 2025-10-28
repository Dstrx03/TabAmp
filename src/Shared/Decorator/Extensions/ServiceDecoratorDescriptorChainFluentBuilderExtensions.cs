namespace TabAmp.Shared.Decorator.Fluent.Extensions;

public static class ServiceDecoratorDescriptorChainFluentBuilderExtensions
{
    public static bool IsEmpty<TService>(this ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : notnull => builder.IsEmpty;

    public static bool IsSingle<TService>(this ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : notnull => builder.IsSingle;

    public static bool IsNormalized<TService>(this ServiceDecoratorDescriptorChainFluentBuilder<TService> builder)
        where TService : notnull => builder.IsNormalized;
}
