namespace TabAmp.IO;

public class SimpleSequentialStreamReader : ISequentialStreamReader
{
    private readonly ITabFileReaderContext _context;
    private readonly FileStream _fileStream;

    public SimpleSequentialStreamReader(ITabFileReaderContext context)
    {
        _context = context;
        _fileStream = OpenFile();
    }

    private FileStream OpenFile()
    {
        var options = new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            BufferSize = 0,
            Share = FileShare.None
        };
        return File.Open(_context.Path, options);
    }

    public async ValueTask<ReadOnlyMemory<byte>> ReadNextBytesAsync(int count)
    {
        var buffer = new byte[count];
        await _fileStream.ReadAsync(buffer, _context.CancellationToken);
        return buffer;
    }

    public void SkipNextBytes(int count) =>
        _fileStream.Position += count;

    public void Dispose()
    {
        _fileStream?.Dispose();
    }
}
