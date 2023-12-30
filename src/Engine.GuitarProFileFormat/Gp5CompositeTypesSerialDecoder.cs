using System;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat;

internal class Gp5CompositeTypesSerialDecoder
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;

    public Gp5CompositeTypesSerialDecoder(ISerialAsynchronousFileReader fileReader, Gp5PrimitivesSerialDecoder primitivesDecoder)
    {
        _fileReader = fileReader;
        _primitivesDecoder = primitivesDecoder;
    }

    public async ValueTask<string> ReadStringAsync(int length, int? size = null)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        SkipStringTrailingBytes(length, size ?? length);
        return Encoding.UTF8.GetString(buffer);
    }

    private void SkipStringTrailingBytes(int length, int size)
    {
        var trailingBytesCount = size - length;
        if (trailingBytesCount > 0)
            _fileReader.SkipBytes(trailingBytesCount);
        else if (trailingBytesCount < 0)
            // TODO: more specific exception type
            throw new InvalidOperationException("String size cannot be less than length.");
    }

    public async ValueTask<string> ReadStringOfByteLengthAsync(int? size = null)
    {
        var length = await _primitivesDecoder.ReadByteAsync();
        return await ReadStringAsync(length, size);
    }

    public async ValueTask<string> ReadStringOfByteLengthIntSizeAsync()
    {
        var size = await _primitivesDecoder.ReadIntZeroBasedAsync();
        return await ReadStringOfByteLengthAsync(size);
    }

    public async ValueTask<string> ReadStringOfIntLengthAsync()
    {
        var length = await _primitivesDecoder.ReadIntAsync();
        return await ReadStringAsync(length);
    }
}
