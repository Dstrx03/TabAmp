namespace TabAmp.IO
{
    public partial class TabFileReader
    {
        private class TabFileReaderContext : ITabFileReaderContext
        {
            public string Path { get; set; } = null;
            public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
        }
    }
}
