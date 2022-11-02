namespace TabAmp.IO;

public interface ITabFileReaderContext
{
    public string Path { get; }
    public TabFileExtension Extension { get; }
    public CancellationToken CancellationToken { get; }
}
