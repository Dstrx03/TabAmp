namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection.Reader;

internal readonly ref struct ReaderOptions
{
    public readonly IIntegrityValidatorDescriptor? IntegrityValidator { get; init; }
}
