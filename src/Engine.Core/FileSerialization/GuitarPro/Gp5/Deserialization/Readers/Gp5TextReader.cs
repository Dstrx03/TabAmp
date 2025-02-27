using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5TextReader : IGp5TextReader
{
    private readonly ISerialFileReader _fileReader;
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;

    public Gp5TextReader(ISerialFileReader fileReader, IGp5BinaryPrimitivesReader primitivesReader) =>
        (_fileReader, _primitivesReader) = (fileReader, primitivesReader);

    public async ValueTask<Gp5ByteText> ReadByteTextAsync(int maxLength)
    {
        var length = await _primitivesReader.ReadByteAsync();
        var text = await ReadStringAsync(length);

        var textWrapper = new Gp5ByteText(length, text, maxLength);

        await _fileReader.SkipBytesAsync(textWrapper.TrailingBytesCount);

        return textWrapper;
    }

    public async ValueTask<string> ReadIntTextAsync()
    {
        var length = await _primitivesReader.ReadIntAsync();
        var text = await ReadStringAsync(length);

        return text;
    }

    public async ValueTask<Gp5IntByteText> ReadIntByteTextAsync()
    {
        var size = await _primitivesReader.ReadIntAsync();
        var length = await _primitivesReader.ReadByteAsync();
        var text = await ReadStringAsync(length);

        return new Gp5IntByteText(size, length, text);
    }

    private ValueTask<string> ReadStringAsync(int length) =>
        _fileReader.ReadBytesAsync(length, ConvertToString);

    private static ISerialFileReader.Convert<string> ConvertToString { get; } = Encoding.UTF8.GetString;
}
