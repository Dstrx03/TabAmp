using System;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;
using TabAmp.Engine.GuitarProFileFormat.Models;

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

    public async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        var length = await _primitivesDecoder.ReadByteAsync();
        var decodedString = await ReadStringAsync(length);

        var trailingBytesCount = maxLength - length;
        if (trailingBytesCount < 0)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{maxLength}-{length}<0 P={_fileReader.Position}");
        else
            await _fileReader.SkipBytesAsync(trailingBytesCount);

        return decodedString;
    }

    public async ValueTask<string> ReadIntStringAsync()
    {
        var length = await _primitivesDecoder.ReadIntAsync();
        return await ReadStringAsync(length);
    }

    public async ValueTask<string> ReadIntByteStringAsync()
    {
        const int lengthPrefixSize = Gp5PrimitivesSerialDecoder.ByteSize;

        var maxLength = await _primitivesDecoder.ReadIntAsync();
        var length = await _primitivesDecoder.ReadByteAsync();

        if (length + lengthPrefixSize != maxLength)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{length}+{lengthPrefixSize}!={maxLength} P={_fileReader.Position}");

        return await ReadStringAsync(length);
    }

    private async ValueTask<string> ReadStringAsync(int length)
    {
        var buffer = await _fileReader.ReadBytesAsync(length);
        return Encoding.UTF8.GetString(buffer);
    }

    public async ValueTask<Gp5RseEqualizer> ReadRseEqualizerAsync(int bandsCount)
    {
        var bands = new sbyte[bandsCount];
        for (var i = 0; i < bands.Length; i++)
        {
            bands[i] = await _primitivesDecoder.ReadSignedByteAsync();
        }

        var gainPreFader = await _primitivesDecoder.ReadSignedByteAsync();

        return new Gp5RseEqualizer
        {
            Bands = bands,
            GainPreFader = gainPreFader
        };
    }
}
