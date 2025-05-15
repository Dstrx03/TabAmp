using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;

internal interface ISerialFileReader : IFileDeserializationMetadataProvider
{
    delegate T Convert<T>(ReadOnlySpan<byte> buffer);

    ValueTask<T> ReadBytesAsync<T>(int count, Convert<T> convert);
    ValueTask SkipBytesAsync(int count);
}
