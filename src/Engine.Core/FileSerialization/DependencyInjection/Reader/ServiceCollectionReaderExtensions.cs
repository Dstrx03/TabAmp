using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Decorator;
using TabAmp.Shared.Decorator.Fluent;
using TabAmp.Shared.Decorator.Fluent.Extensions;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public static class ServiceCollectionReaderExtensions
{
    public static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection,
        ReaderOptions<TService, TReader> options)
        where TService : class
        where TReader : notnull, TService
    {
        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService>(isNormalized: true);
        options.IntegrityValidator?.Append(builder, out builder);

        if (builder.IsEmpty())
            throw null;

        return serviceCollection.AddDecoratedScoped<TService, TReader>(builder);
    }
}
