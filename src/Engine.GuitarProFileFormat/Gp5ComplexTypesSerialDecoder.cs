using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat;

public class Gp5ComplexTypesSerialDecoder
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;

    public Gp5ComplexTypesSerialDecoder(ISerialAsynchronousFileReader fileReader, Gp5PrimitivesSerialDecoder primitivesDecoder)
    {
        _fileReader = fileReader;
        _primitivesDecoder = primitivesDecoder;
    }

    public async ValueTask<string> ReadStringAsync(int size)
    {
        var buffer = await _fileReader.ReadBytesAsync(size);
        return Encoding.UTF8.GetString(buffer);
    }

    public async ValueTask<string> ReadByteSizeStringAsync()
    {
        var size = await _primitivesDecoder.ReadByteAsync();
        return await ReadStringAsync(size);
    }

    public async ValueTask<string> ReadIntSizeStringAsync()
    {
        var size = await _primitivesDecoder.ReadIntAsync();
        return await ReadStringAsync(size);
    }

    public async ValueTask<string> ReadIntByteSizeStringAsync()
    {
        var intValue = await _primitivesDecoder.ReadIntZeroBasedAsync();
        var size = await _primitivesDecoder.ReadByteAsync();

        var stringValue = await ReadStringAsync(size);

        var skipBytes = intValue - size;
        if (skipBytes > 0)
            _fileReader.SkipBytes(skipBytes);

        return stringValue;
    }
}
