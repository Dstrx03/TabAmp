using System;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;

internal interface ISerialFileReader : IDisposable
{
    long Length { get; }
    long Position { get; }

    delegate T TODO_Delegate_Name<T>(ReadOnlySpan<byte> buffer);

    ValueTask<byte[]> ReadBytesAsync(int count);
    ValueTask SkipBytesAsync(int count);
}
