using System;
using System.IO;
using System.Threading.Tasks;

namespace TabAmp.Engine.GuitarProFileFormat.FileReader;

public class PocSerialAsynchronousFileReader : ISerialAsynchronousFileReader
{
    private readonly FileStream _fileStream;

    public PocSerialAsynchronousFileReader(string filePath, FileStreamOptions options = null)
    {
        options ??= new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            //BufferSize = 0,
            Share = FileShare.None
        };
        _fileStream = File.Open(filePath, options);
    }

    public long Length => _fileStream.Length;
    public long Position => _fileStream.Position;

    public async ValueTask<byte[]> ReadBytesAsync(int count)
    {
        var buffer = new byte[count];
        await _fileStream.ReadAsync(buffer, default);
        return buffer;
    }

    public async ValueTask SkipBytesAsync(int count)
    {
        var skippedBytes = await ReadBytesAsync(count);
        Console.WriteLine($"Skipped {count} bytes from {Position - count} to {Position - 1} inclusive: {string.Join(",", skippedBytes)}");

        // TODO: implement production grade tracking of skipped information
        //_fileStream.Position += count;
    }

    public void Dispose()
    {
        Console.WriteLine($"Disposing POC file reader, read {Position} of {Length} bytes");
        // TODO: implement production grade tracking

        _fileStream?.Dispose();
    }
}
