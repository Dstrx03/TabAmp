using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator;

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
        ReaderOptions options)
        where TService : class
        where TReader : class, TService
    {
        var builder = ConfigureDescriptorChain.For<TService, TReader>();
        return serviceCollection.AddReader<TService, TReader>();
    }
}
