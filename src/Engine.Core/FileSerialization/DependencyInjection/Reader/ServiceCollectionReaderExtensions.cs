using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Decorator;
using TabAmp.Shared.Decorator.Fluent;
using TabAmp.Shared.Decorator.Fluent.Extensions;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal static class ServiceCollectionReaderExtensions
{
    public static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection,
        ReaderOptions<TService, TReader> options)
        where TService : class
        where TReader : notnull, TService
    {
        var builder = NormalizedDescriptorChain.For<TService, TReader>().With(options.IntegrityValidator);

        if (builder.IsEmpty())
            throw null;

        return serviceCollection.AddDecoratedScoped(builder);
    }

    private static ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> With<TService, TReader>(
        this ServiceDecoratorDescriptorChainFluentBuilder<TService, TReader> builder,
        ReaderDecoratorDescriptor<TService, TReader>? descriptor)
        where TService : notnull
        where TReader : notnull, TService
    {
        if (descriptor is null)
            return builder;

        return descriptor.AppendTo(builder);
    }
}
