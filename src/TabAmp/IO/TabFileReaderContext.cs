namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private class TabFileReaderContext : ITabFileReaderContext
    {
        public string FilePath { get; set; } = null;
        public TabFileExtension FileExtension { get; set; } = TabFileExtension.Other;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        public bool IsSigned { get; private set; }
        public void Sign() =>
            IsSigned = true;
    }
}
