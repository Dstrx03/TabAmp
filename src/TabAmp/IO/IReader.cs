namespace TabAmp.IO
{
    public interface IReader : IDisposable
    {
        public void Open(string path);
        public ValueTask<ReadOnlyMemory<byte>> ReadBytesSequenceAsync(int count);
        public void SkipBytesSequence(int count);
    }
}
