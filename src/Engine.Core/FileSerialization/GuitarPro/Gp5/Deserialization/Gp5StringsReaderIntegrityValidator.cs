using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5StringsReaderIntegrityValidator : IGp5StringsReader
{
    private readonly IGp5StringsReader _stringsReader;

    public Gp5StringsReaderIntegrityValidator(IGp5StringsReader stringsReader) =>
        _stringsReader = stringsReader;

    public async ValueTask<Gp5ByteString> ReadByteStringAsync(int maxLength)
    {
        var stringValue = await _stringsReader.ReadByteStringAsync(maxLength);

        if (stringValue.TrailingBytesCount < 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"{maxLength}-{stringValue.DecodedString.Length}<0 P=~");

        return stringValue;
    }

    public ValueTask<string> ReadIntStringAsync() =>
        _stringsReader.ReadIntStringAsync();

    public async ValueTask<Gp5IntByteString> ReadIntByteStringAsync()
    {
        var stringValue = await _stringsReader.ReadIntByteStringAsync();

        if (stringValue.DecodedString.Length + Gp5IntByteString.LengthByteSize != stringValue.MaxLength)
            // TODO: message
            throw new FileSerializationIntegrityException($"{stringValue.DecodedString.Length}+{Gp5IntByteString.LengthByteSize}!={stringValue.MaxLength} P=~");

        return stringValue;
    }
}
