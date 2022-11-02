namespace TabAmp.IO;

public partial class TabFileReader
{
    private class TabFileReaderContext : ITabFileReaderContext
    {
        public string FilePath { get; set; } = null;
        public TabFileExtension FileExtension { get; set; } = TabFileExtension.Other;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
