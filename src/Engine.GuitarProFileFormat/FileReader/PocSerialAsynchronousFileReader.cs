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

    public void SkipBytes(int count) =>
        _fileStream.Position += count;

    public void Dispose() =>
        _fileStream?.Dispose();
}
