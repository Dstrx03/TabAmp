namespace TabAmp.IO;

public interface ITabFileReaderContext
{
    public string FilePath { get; }
    public TabFileExtension FileExtension { get; }
    public CancellationToken CancellationToken { get; }
}
