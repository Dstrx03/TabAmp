namespace TabAmp.IO
{
    public interface ITabFileReaderContext
    {
        public string Path { get; }
        public CancellationToken CancellationToken { get; }
    }
}
