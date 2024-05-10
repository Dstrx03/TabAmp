using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5StringsReader : IGp5StringsReader
{
    private readonly ISerialFileReader _fileReader;
    private readonly IGp5BinaryPrimitivesReader _binaryPrimitivesReader;

    public Gp5StringsReader(ISerialFileReader fileReader, IGp5BinaryPrimitivesReader binaryPrimitivesReader)
    {
        _fileReader = fileReader;
        _binaryPrimitivesReader = binaryPrimitivesReader;
    }

    public async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        var length = await _binaryPrimitivesReader.ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        var trailingBytesCount = maxLength - length;
        if (trailingBytesCount > 0)
            await _fileReader.SkipBytesAsync(trailingBytesCount);
        else if (trailingBytesCount < 0)
            // TODO: message
            throw new FileSerializationIntegrityException($"{maxLength}-{length}<0 P={_fileReader.Position}");

        return decodedString;
    }

    public async ValueTask<string> ReadIntStringAsync()
    {
        var length = await _binaryPrimitivesReader.ReadIntAsync();
        return await ReadStringAsync(length);
    }

    public async ValueTask<string> ReadIntByteStringAsync()
    {
        var maxLength = await _binaryPrimitivesReader.ReadIntAsync();
        var length = await _binaryPrimitivesReader.ReadByteAsync();

        const int ByteSize = 1;
        if (length + ByteSize != maxLength)
            // TODO: message
            throw new FileSerializationIntegrityException($"{length}+{ByteSize}!={maxLength} P={_fileReader.Position}");

        return await ReadStringAsync(length);
    }

    private async ValueTask<string> ReadStringAsync(int length)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        return Encoding.UTF8.GetString(buffer);
    }
}
