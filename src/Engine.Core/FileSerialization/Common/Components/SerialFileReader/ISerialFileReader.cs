using System;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;

internal interface ISerialFileReader : IDisposable
{
    long Length { get; }
    long Position { get; }

    delegate T Convert<T>(ReadOnlySpan<byte> buffer);

    ValueTask<T> ReadBytesAsync<T>(int count, Convert<T> convert);
    ValueTask SkipBytesAsync(int count);
}
