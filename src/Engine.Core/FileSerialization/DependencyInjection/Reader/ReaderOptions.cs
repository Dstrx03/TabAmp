namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

public static class ReaderOptions
{
    public static ReaderOptions<TService, TReader> For<TService, TReader>()
        where TService : notnull
        where TReader : notnull, TService => new();
}

public readonly ref struct ReaderOptions<TService, TReader>
    where TService : notnull
    where TReader : notnull, TService
{
    public readonly IntegrityValidatorDescriptor<TService, TReader>? IntegrityValidator { get; init; }

    public ReaderOptions<TService, TReader> WithIntegrityValidator<TIntegrityValidator>()
        where TIntegrityValidator : notnull, TService
    {
        return new()
        {
            IntegrityValidator = new IntegrityValidatorDescriptor<TService, TReader>.For<TIntegrityValidator>()
        };
    }
}
