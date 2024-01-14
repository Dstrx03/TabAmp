﻿using System.Buffers.Binary;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat;

internal class Gp5PrimitivesSerialDecoder
{
    private const int ByteSize = 1;
    private const int SignedByteSize = 1;
    private const int BoolSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;

    private readonly ISerialAsynchronousFileReader _fileReader;

    public Gp5PrimitivesSerialDecoder(ISerialAsynchronousFileReader fileReader)
    {
        _fileReader = fileReader;
    }

    public async ValueTask<byte> ReadByteAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ByteSize);
        return buffer[0];
    }

    public async ValueTask<sbyte> ReadSignedByteAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(SignedByteSize);
        return (sbyte)buffer[0];
    }

    public async ValueTask<bool> ReadBoolAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(BoolSize);
        return buffer[0] == 1;
    }

    public async ValueTask<short> ReadShortAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ShortSize);
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }

    public async ValueTask<int> ReadIntAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(IntSize);
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }

    public async ValueTask<int> ReadIntZeroBasedAsync() => 
        await ReadIntAsync() - 1;

    public async ValueTask<float> ReadFloatAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(FloatSize);
        return BinaryPrimitives.ReadSingleLittleEndian(buffer);
    }

    public async ValueTask<double> ReadDoubleAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(DoubleSize);
        return BinaryPrimitives.ReadDoubleLittleEndian(buffer);
    }
}