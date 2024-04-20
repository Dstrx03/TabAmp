using System.Buffers.Binary;
using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5TypesReader
{
    private readonly ISerialFileReader _fileReader;

    private const int ByteSize = 1;
    private const int ShortSize = 2;
    private const int IntSize = 4;
    private const int FloatSize = 4;
    private const int DoubleSize = 8;

    private const byte BoolFalseValue = 0;
    private const byte BoolTrueValue = 1;

    public Gp5TypesReader(ISerialFileReader fileReader) =>
        _fileReader = fileReader;

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
        var byteValue = await ReadByteAsync();

        if (byteValue is not BoolFalseValue and not BoolTrueValue)
            // TODO: message
            throw new FileSerializationIntegrityException($"{byteValue}!=0<>1 P={_fileReader.Position}");

        return byteValue == BoolTrueValue;
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

    public async ValueTask<string> ReadByteStringAsync(int maxLength)
    {
        var length = await ReadByteAsync();
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
        var length = await ReadIntAsync();
        return await ReadStringAsync(length);
    }

    public async ValueTask<string> ReadIntByteStringAsync()
    {
        var maxLength = await ReadIntAsync();
        var length = await ReadByteAsync();

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
