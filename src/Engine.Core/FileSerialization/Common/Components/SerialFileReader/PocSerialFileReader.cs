using System;
using System.IO;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;

internal class PocSerialFileReader : ISerialFileReader
{
    private readonly FileStream _fileStream;
    private readonly FileSerializationContext _context;

    public PocSerialFileReader(FileSerializationContext context)
    {
        var options = new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            Share = FileShare.None
        };
        _fileStream = File.Open(context.FilePath, options);
        _context = context;
    }

    public long Length => _fileStream.Length;
    public long Position => _fileStream.Position;

    public async ValueTask<byte[]> ReadBytesAsync(int count)
    {
        var buffer = new byte[count];
        await _fileStream.ReadAsync(buffer, _context.CancellationToken);
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
