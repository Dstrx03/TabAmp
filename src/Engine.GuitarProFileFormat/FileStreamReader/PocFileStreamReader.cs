using System;
using System.IO;
using System.Threading.Tasks;

namespace TabAmp.Engine.GuitarProFileFormat.FileStreamReader;

public class PocFileStreamReader : IFileStreamReader, IDisposable
{
    private readonly FileStream _fileStream;

    public PocFileStreamReader(string filePath, FileStreamOptions options = null)
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

    public async ValueTask<byte[]> ReadNextAsync(int bytesCount)
    {
        var buffer = new byte[bytesCount];
        await _fileStream.ReadAsync(buffer, default);
        return buffer;
    }

    public void SkipNext(int bytesCount) =>
        _fileStream.Position += bytesCount;

    public void Dispose() =>
        _fileStream?.Dispose();
}
