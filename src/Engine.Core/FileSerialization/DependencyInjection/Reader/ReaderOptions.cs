namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal static class ReaderOptions
{
    public static ReaderOptions<TService, TReader> For<TService, TReader>()
        where TService : class
        where TReader : class, TService => new();
}

internal readonly ref struct ReaderOptions<TService, TReader>
    where TService : class
    where TReader : class, TService
{
    public readonly IIntegrityValidatorDescriptor<TService>? IntegrityValidator { get; init; }

    public ReaderOptions<TService, TReader> WithIntegrityValidator<TIntegrityValidator>()
        where TIntegrityValidator : class, TService
    {
        return new()
        {
            IntegrityValidator = new IIntegrityValidatorDescriptor<TService>.For<TIntegrityValidator>()
        };
    }
}
