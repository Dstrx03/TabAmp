using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Decorator;
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
        var builder = NormalizedDescriptorChain.For<TService, TReader>();
        options.IntegrityValidator?.AppendTo(builder, out builder);

        if (builder.IsEmpty())
            throw null;

        return serviceCollection.AddDecoratedScoped<TService, TReader>(builder);
    }
}
