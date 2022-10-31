namespace TabAmp.IO;

public interface ISequentialStreamReader : IDisposable
{
    public ValueTask<ReadOnlyMemory<byte>> ReadNextBytesAsync(int count);
    public void SkipNextBytes(int count);
}
