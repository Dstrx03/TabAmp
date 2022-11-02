namespace TabAmp.IO;

public partial class TabFileReader
{
    private class TabFileReaderContext : ITabFileReaderContext
    {
        public string Path { get; set; } = null;
        public TabFileExtension Extension { get; set; } = TabFileExtension.Other;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
