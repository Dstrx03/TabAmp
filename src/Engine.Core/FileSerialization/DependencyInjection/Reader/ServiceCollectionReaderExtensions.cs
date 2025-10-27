using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public static class ServiceCollectionReaderExtensions
{
    public static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection,
        ReaderOptions<TService, TReader> options)
        where TService : class
        where TReader : notnull, TService
    {
        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService>();

        var integrityValidator = options.IntegrityValidator;
        integrityValidator?.AppendToDescriptorChain(builder, out builder);

        return null;
    }
}
