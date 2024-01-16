using System;
using System.Threading.Tasks;

namespace TabAmp.Engine.GuitarProFileFormat.FileReader;

public interface ISerialAsynchronousFileReader : IDisposable
{
    long Length { get; }
    long Position { get; }

    ValueTask<byte[]> ReadBytesAsync(int count);
    ValueTask SkipBytesAsync(int count);
}
