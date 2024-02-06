using System;
using System.Buffers.Binary;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5PrimitivesSerialDecoder
{
    public const int ByteSize = 1;
    public const int ShortSize = 2;
    public const int IntSize = 4;
    public const int FloatSize = 4;
    public const int DoubleSize = 8;

    private readonly ISerialFileReader _fileReader;

    public Gp5PrimitivesSerialDecoder(ISerialFileReader fileReader)
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
        var byteValue = await ReadByteAsync();
        return (sbyte)byteValue;
    }

    public async ValueTask<bool> ReadBoolAsync()
    {
        const byte falseValue = 0;
        const byte trueValue = 1;

        var byteValue = await ReadByteAsync();

        if (byteValue != falseValue && byteValue != trueValue)
            // TODO: more specific exception type, message
            throw new InvalidOperationException($"{byteValue}!=0<>1 P={_fileReader.Position}");

        return byteValue == trueValue;
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

    public async ValueTask<int> ReadIntZeroBasedAsync()
    {
        var intValue = await ReadIntAsync();
        return intValue - 1;
    }

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
