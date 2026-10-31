namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal readonly ref struct ReaderOptions<TService>
    where TService : class
{
    public readonly IIntegrityValidatorDescriptor<TService>? IntegrityValidator { get; init; }
}
