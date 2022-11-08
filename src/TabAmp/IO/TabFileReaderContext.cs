namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private class TabFileReaderContext : ITabFileReaderContext
    {
        public string FilePath { get; set; } = null;
        public TabFileExtension FileExtension { get; set; } = TabFileExtension.Other;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        public bool Signed { get; private set; } = false;
        public void Sign() =>
            Signed = true;
    }
}
