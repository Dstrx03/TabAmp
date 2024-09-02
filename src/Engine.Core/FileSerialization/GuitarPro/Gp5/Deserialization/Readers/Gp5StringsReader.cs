using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5StringsReader : IGp5StringsReader
{
    private readonly ISerialFileReader _fileReader;
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5StringsReader(ISerialFileReader fileReader, IGp5BinaryPrimitivesReader primitivesReader)
    {
        _fileReader = fileReader;
        _primitivesReader = primitivesReader;
    }

    public async ValueTask<Gp5ByteString> ReadByteStringAsync(int maxLength)
    {
        var length = await _primitivesReader.ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        var stringWrapper = new Gp5ByteString(decodedString, maxLength);
        if (stringWrapper.TrailingBytesCount > 0)
            await _fileReader.SkipBytesAsync(stringWrapper.TrailingBytesCount);

        return stringWrapper;
    }

    public async ValueTask<string> ReadIntStringAsync()
    {
        var length = await _primitivesReader.ReadIntAsync();
        return await ReadStringAsync(length);
    }

    public async ValueTask<Gp5IntByteString> ReadIntByteStringAsync()
    {
        var size = await _primitivesReader.ReadIntAsync();
        var length = await _primitivesReader.ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        return new Gp5IntByteString(decodedString, size);
    }

    private async ValueTask<string> ReadStringAsync(int length)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        return Encoding.UTF8.GetString(buffer);
    }
}
