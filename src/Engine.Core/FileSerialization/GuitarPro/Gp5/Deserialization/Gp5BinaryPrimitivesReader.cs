using System.Buffers.Binary;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5BinaryPrimitivesReader : IGp5BinaryPrimitivesReader
{
    private readonly ISerialFileReader _fileReader;

    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;

    private const byte BoolFalseValue = 0;
    private const byte BoolTrueValue = 1;

    public Gp5BinaryPrimitivesReader(ISerialFileReader fileReader) =>
        _fileReader = fileReader;

    public ValueTask<byte> ReadByteAsync() =>
        ReadByteValueAsync();

    public async ValueTask<sbyte> ReadSignedByteAsync()
    {
        var byteValue = await ReadByteValueAsync();
        return (sbyte)byteValue;
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

    public async ValueTask<bool> ReadBoolAsync()
    {
        var byteValue = await ReadByteValueAsync();

        if (byteValue is not BoolFalseValue and not BoolTrueValue)
            // TODO: message
            throw new FileSerializationIntegrityException($"{byteValue}!=0<>1 P={_fileReader.Position}");

        return byteValue == BoolTrueValue;
    }

    private async ValueTask<byte> ReadByteValueAsync()
    {
        var buffer = await _fileReader.ReadBytesAsync(ByteSize);
        return buffer[0];
    }
}
