using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5StringsReaderIntegrityValidator : IGp5StringsReader
{
    private readonly IGp5StringsReader _stringsReader;

    public Gp5StringsReaderIntegrityValidator(IGp5StringsReader stringsReader) =>
        _stringsReader = stringsReader;

    public async ValueTask<Gp5ByteString> ReadByteStringAsync(int maxLength)
    {
        var stringWrapper = await _stringsReader.ReadByteStringAsync(maxLength);

        if (stringWrapper.TrailingBytesCount < 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"{maxLength}-{stringWrapper.Length}<0 P=~");

        return stringWrapper;
    }

    public ValueTask<string> ReadIntStringAsync() =>
        _stringsReader.ReadIntStringAsync();

    public async ValueTask<Gp5IntByteString> ReadIntByteStringAsync()
    {
        var stringWrapper = await _stringsReader.ReadIntByteStringAsync();

        if (stringWrapper.Length != stringWrapper.MaxLength)
            // TODO: message
            throw new FileSerializationIntegrityException($"{stringWrapper.Length}+1!={stringWrapper.Size} P=~");

        return stringWrapper;
    }
}
