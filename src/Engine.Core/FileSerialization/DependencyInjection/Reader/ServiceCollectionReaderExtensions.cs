using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator;
using TabAmp.Shared.Decorator.Fluent.Extensions;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal static class ServiceCollectionReaderExtensions
{
    public static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TReader : class, TService
    {
        return serviceCollection.AddScoped<TService, TReader>();
    }

    public static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection,
        ReaderOptions<TService> options)
        where TService : class
        where TReader : class, TService
    {
        var builder = ConfigureDescriptorChain.For<TService, TReader>().With(options.IntegrityValidator?.Descriptor);
        return serviceCollection.AddReader<TService, TReader>();
    }
}
